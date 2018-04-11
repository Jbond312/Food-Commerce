using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Exceptions;
using Email.Service.Business.Helper;
using Foods.Service.Intercom.Email;
using Microsoft.AspNetCore.Identity;
using SparkPost;
using Microsoft.Extensions.Configuration;

namespace Foods.Service.Api.Jwt.Helpers
{
    public class AccountHelper : IAccountHelper
    {
        private readonly UserManager<FoodsIdentityUser> _userManager;
        private readonly IEmailIntercom _emailIntercom;
        private readonly IConfiguration _configuration;

        public AccountHelper(
            UserManager<FoodsIdentityUser> userManager,
            IEmailIntercom emailIntercom,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _emailIntercom = emailIntercom;
            _configuration = configuration;
        }

        public async Task ValidateCurrentPasswordIsCorrect(FoodsIdentityUser user, string passwordToValidate)
        {
            var isCorrectPassword = await _userManager.CheckPasswordAsync(user, passwordToValidate);

            if (!isCorrectPassword)
            {
                throw new FoodsValidationException("CurrentPassword", "Obfuscated",
                    "Current password is incorrect");
            }
        }

        public async Task ChangePassword(FoodsIdentityUser user, string oldPassword, string newPassword)
        {
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => new FoodsError
                {
                    PropertyName = "Username/Password",
                    ErrorMessage = error.Description
                })
                    .ToList();

                throw new FoodsValidationException(errors);
            }
        }

        public async Task SendResetPasswordEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new FoodsValidationException("Email", "", "There was an issuing when resetting the password. The password reset request has not been sent.");
            }

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

            var generatedUrl = $"{_configuration["BaseAppUrl"]}/confirm-password?reset_token={resetToken}&user_id={user.Id}";

            await _emailIntercom.Send(EmailTypes.PasswordReset, new TransmissionPayload
            {
                Destinations = new List<TransmissionDestination>
                {
                    new TransmissionDestination
                    {
                        DestinationEmail = email,
                        SubstitutionData = new Dictionary<string, object>
                        {
                            ["token"] = generatedUrl
                        }
                    }
                }
            });
        }

        public async Task SendResetPasswordSuccessEmail(FoodsIdentityUser user)
        {
            await _emailIntercom.Send(EmailTypes.AccountDetailsUpdated, new TransmissionPayload
            {
                Destinations = new List<TransmissionDestination>
                {
                    new TransmissionDestination
                    {
                        DestinationEmail = user.Email,
                        SubstitutionData = new Dictionary<string, object>
                        {
                            ["updatedAccountDetails"] = "Password changed."
                        }
                    }
                }
            });
        }

        public async Task ChangeEmail(FoodsIdentityUser user, string newEmail)
        {
            ValidateEmailNotAlreadyInUse(newEmail);

            user.PreviousEmail = user.Email;
            user.EmailConfirmed = false;

            await _userManager.SetEmailAsync(user, newEmail);
            await _userManager.SetUserNameAsync(user, newEmail);
            await _userManager.UpdateAsync(user);

            //Invalidate links sent to other accounts
            await _userManager.UpdateSecurityStampAsync(user);

            await SendEmailConfirmation(user);
            await SendEmailChangedNotification(user.PreviousEmail);
        }

        private void ValidateEmailNotAlreadyInUse(string email)
        {
            var existingUser = _userManager.Users.SingleOrDefault(r => string.Equals(r.Email, email,
                StringComparison.CurrentCultureIgnoreCase));

            if (existingUser != null)
            {
                throw new FoodsValidationException("Email", email,
                    "Cannot change e-mail. It is already in use.");
            }
        }

        public async Task SendEmailConfirmation(FoodsIdentityUser user)
        {
            var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var generatedUrl = $"{_configuration["BaseAppUrl"]}/confirm-email?confirm_token={emailConfirmToken}&user_id={user.Id}";

            await _emailIntercom.Send(EmailTypes.EmailConfirmation, new TransmissionPayload
            {
                Destinations = new List<TransmissionDestination>
                {
                    new TransmissionDestination
                    {
                        DestinationEmail = user.Email,
                        SubstitutionData = new Dictionary<string, object>
                        {
                            ["confirmationUrl"] = generatedUrl
                        }
                    }
                },
                Attachments = new List<Attachment>()
            });
        }

        private async Task SendEmailChangedNotification(string email)
        {
            await _emailIntercom.Send(EmailTypes.AccountDetailsUpdated, new TransmissionPayload
            {
                Destinations = new List<TransmissionDestination>
                {
                    new TransmissionDestination
                    {
                        DestinationEmail = email,
                        SubstitutionData = new Dictionary<string, object>
                        {
                            ["updatedAccountDetails"] = "E-mail address changed."
                        }
                    }
                }
            });
        }
    }
}