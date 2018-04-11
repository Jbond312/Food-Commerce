using FluentValidation;

namespace Foods.Service.Api.Jwt.Entities.Validators
{
    public class LoginValidator : BaseAccountValidator<LoginDto>
    {
        public LoginValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleFor(x => x.Email).Cascade(CascadeMode.StopOnFirstFailure).NotEmpty().EmailAddress().Must(EmailHasValidLength).WithMessage("Email address is an invalid length");
            RuleFor(x => x.Password).NotEmpty();
        }
    }
}
