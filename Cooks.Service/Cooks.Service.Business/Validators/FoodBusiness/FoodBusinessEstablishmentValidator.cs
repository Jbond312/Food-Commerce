using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class FoodBusinessEstablishmentValidator : EntityValidator<FoodBusinessEstablishmentDto>
    {
        private readonly IEntityValidator<AddressDto> _addressValidator;

        public FoodBusinessEstablishmentValidator(IEntityValidator<AddressDto> addressValidator)
        {
            _addressValidator = addressValidator;

            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                RuleFor(fbe => fbe.FoodBusinessName).NotEmpty();
                RuleFor(fbe => fbe.EstablishmentAddress).SetValidator(_addressValidator.Validator);

                SetCommonValidationRules();
            });
        }

        public void SetCommonValidationRules()
        {
            When(fbe => fbe.IsSameAddress, () =>
            {
                RuleFor(fbe => fbe.EstablishmentAddress).Null();
            });
        }
    }
}
