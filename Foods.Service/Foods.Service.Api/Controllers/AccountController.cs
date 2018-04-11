using System;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Common.Repository;
using Common.Validation;
using Foods.Service.Api.Jwt;
using Foods.Service.Api.Jwt.Entities;
using Foods.Service.Api.Jwt.Helpers;
using Foods.Service.Repository.Cooks;
using Foods.Service.Repository.Users;
using Foods.Service.Repository.Users.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Foods.Service.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Authorize]
    public class AccountController : BaseController
    {
        private readonly SignInManager<FoodsIdentityUser> _signInManager;
        private readonly UserManager<FoodsIdentityUser> _userManager;
        private readonly IUserRepository<User> _userRepository;
        private readonly IRepository<Cook> _cookRepository;
        private readonly IAccountHelper _accountHelper;
        private readonly IJwtHelper _jwtHelper;
        private readonly IEntityValidator<LoginDto> _loginValidator;
        private readonly IEntityValidator<RegisterDto> _registerValidator;

        public AccountController(
            UserManager<FoodsIdentityUser> userManager,
            SignInManager<FoodsIdentityUser> signInManager,
            IUserRepository<User> userRepository,
            IRepository<Cook> cookRepository,
            IAccountHelper accountHelper,
            IJwtHelper jwtHelper,
            IEntityValidator<LoginDto> loginValidator,
            IEntityValidator<RegisterDto> registerValidator)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userRepository = userRepository;
            _cookRepository = cookRepository;
            _accountHelper = accountHelper;
            _jwtHelper = jwtHelper;
            _loginValidator = loginValidator;
            _registerValidator = registerValidator;
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            async Task<string> Login()
            {
                await _loginValidator.ValidateEntityAsync(loginDto);

                var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, false, false);

                if (!result.Succeeded)
                {
                    throw new FoodsValidationException("Username", loginDto.Email, "Invalid username or password");
                }

                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == loginDto.Email);
                var jwtToken = _jwtHelper.GenerateToken(appUser);
                return jwtToken;
            }

            return await Execute(Login);

        }

        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            async Task SaveUser(FoodsIdentityUser user)
            {
                
                var foodsUser = await _userRepository.Save(new User
                {
                    Id = Guid.NewGuid().ToString(),
                    IdentityProviderId = user.Id,
                    Email = user.Email
                });

                user.UserId = foodsUser.Id;
            }

            async Task SaveCook(FoodsIdentityUser user)
            {
                var foodsCook = await _cookRepository.Save(new Cook
                {
                    Id = Guid.NewGuid().ToString()
                });

                user.CookId = foodsCook.Id;
            }

            async Task<string> Register()
            {
                await _registerValidator.ValidateEntityAsync(registerDto);

                var user = new FoodsIdentityUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email
                };

                var result = await _userManager.CreateAsync(user, registerDto.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    await SaveUser(user);
                    await SaveCook(user);
                    await _userManager.UpdateAsync(user);
                    await _accountHelper.SendEmailConfirmation(user);

                    var jwtToken = _jwtHelper.GenerateToken(user);

                    return jwtToken;
                }

                var errors = result.Errors.Select(error => new FoodsError
                    {
                        PropertyName = "Username/Password",
                        ErrorMessage = error.Description
                    })
                    .ToList();

                throw new FoodsValidationException(errors);
            }

            return await Execute(Register);
        }

        [HttpPut]
        [Route("UpdateAccount")]
        public async Task<IActionResult> UpdateAccount([FromBody]UserAccountDto userAccountDto)
        {
            async Task UpdateAccount()
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);

                await _accountHelper.ValidateCurrentPasswordIsCorrect(user, userAccountDto.CurrentPassword);

                if (!string.IsNullOrEmpty(userAccountDto.NewPassword))
                {
                    await _accountHelper.ChangePassword(user, userAccountDto.CurrentPassword,
                        userAccountDto.NewPassword);
                }

                bool EmailHasChanged()
                {
                    return !string.IsNullOrEmpty(userAccountDto.Email) && !string.Equals(user.Email, userAccountDto.Email, StringComparison.CurrentCultureIgnoreCase);
                }

                if (EmailHasChanged())
                {
                    await _accountHelper.ChangeEmail(user, userAccountDto.Email);
                }
            }

            return await Execute(UpdateAccount);
        }

        [HttpPost]
        [Route("ResetPassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordDto resetPasswordDto)
        {
            async Task ResetPassword()
            {
                await _accountHelper.SendResetPasswordEmail(resetPasswordDto.Email);
            }

            return await Execute(ResetPassword);
        }

        [HttpPost]
        [Route("ResetPasswordConfirm")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPasswordConfirm([FromBody]ResetPasswordConfirmDto resetPasswordDto)
        {
            async Task ResetPassword()
            {
                bool PasswordsDoNotMatch()
                {
                    return resetPasswordDto.NewPassword != null && resetPasswordDto.NewPassword != resetPasswordDto.ConfirmPassword;
                }
                
                if (PasswordsDoNotMatch())
                {
                    throw new FoodsValidationException("Password", "", "The new password and the confirmed password do not match.");
                }

                var user = await _userManager.FindByIdAsync(resetPasswordDto.IdentityId);

                bool HasEnoughDetailsToResetPassword()
                {
                    return user != null && !string.IsNullOrEmpty(resetPasswordDto.NewPassword) && !string.IsNullOrEmpty(resetPasswordDto.ConfirmPassword);
                }

                if (!HasEnoughDetailsToResetPassword())
                {
                    throw new FoodsValidationException("Email", "",
                        "There was an issuing when resetting the password. The password has not been changed");
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.ResetToken, resetPasswordDto.NewPassword);

                CheckIdentityResult(result, "Username/Password");

                await _accountHelper.SendResetPasswordSuccessEmail(user);
            }

            return await Execute(ResetPassword);
        }

        [HttpPost]
        [Route("ConfirmEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            async Task ConfirmEmail()
            {
                if (string.IsNullOrEmpty(confirmEmailDto.IdentityId) || string.IsNullOrEmpty(confirmEmailDto.EmailConfirmToken))
                {
                    throw new FoodsValidationException("UserId/EmailToken", "", "The user id and email token are required.");
                }

                var user = await _userManager.FindByIdAsync(confirmEmailDto.IdentityId);

                if (user == null)
                {
                    throw new FoodsValidationException("UserId", "", "Unable to find user");
                }

                var result = await _userManager.ConfirmEmailAsync(user, confirmEmailDto.EmailConfirmToken);

                CheckIdentityResult(result, "UserId/EmailToken");

                result = await _userManager.UpdateSecurityStampAsync(user);

                CheckIdentityResult(result, "User");
            }

            return await Execute(ConfirmEmail);
        }

        private void CheckIdentityResult(IdentityResult result, string propertyName, string attemptedValue = null)
        {
            if (result.Succeeded) return;

            var errors = result.Errors.Select(error => new FoodsError
                {
                    PropertyName = propertyName,
                    ErrorMessage = error.Description,
                    AttemptedValue = attemptedValue
            })
                .ToList();

            throw new FoodsValidationException(errors);
        }
    }
}