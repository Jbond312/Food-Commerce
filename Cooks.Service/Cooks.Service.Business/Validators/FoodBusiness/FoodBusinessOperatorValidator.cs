using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class FoodBusinessOperatorValidator : EntityValidator<FoodBusinessOperatorDto>
    {
        private readonly IEntityValidator<AddressDto> _addressValidator;

        public FoodBusinessOperatorValidator(IEntityValidator<AddressDto> addressValidator)
        {
            _addressValidator = addressValidator;

            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                RuleFor(fbo => fbo.FirstName).NotEmpty();
                RuleFor(fbo => fbo.LastName).NotEmpty();
                RuleFor(fbo => fbo.Address).SetValidator(_addressValidator.Validator);
                RuleFor(fbo => fbo.Email).NotEmpty().EmailAddress();
                RuleFor(fbo => fbo.TelephoneNumber).NotEmpty();
            });
        }
    }
}
