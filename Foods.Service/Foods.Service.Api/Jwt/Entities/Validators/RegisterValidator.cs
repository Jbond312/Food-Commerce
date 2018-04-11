using FluentValidation;

namespace Foods.Service.Api.Jwt.Entities.Validators
{
    public class RegisterValidator : BaseAccountValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().EmailAddress().Must(EmailHasValidLength).WithMessage("Email address is an invalid length");
            RuleFor(x => x.Password).NotEmpty().Must(HasDifferentPasswordToEmail).WithMessage("The password should be different to the e-mail address.");
        }

        private static bool HasDifferentPasswordToEmail(RegisterDto registerDto, string password)
        {
            return !password.ToLower().Contains(registerDto.Email.ToLower());
        }
    }
}
