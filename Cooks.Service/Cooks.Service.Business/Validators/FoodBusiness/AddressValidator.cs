using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class AddressValidator : EntityValidator<AddressDto>
    {
        public AddressValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                CreateCommonValidationRules();
            });

            RuleSet(RuleSets.Cooks.ToString(), () =>
            {
                CreateCommonValidationRules();
            });
        }

        private void CreateCommonValidationRules()
        {
            RuleFor(a => a.HouseNumber).NotEmpty();
            RuleFor(a => a.Line1).NotEmpty();
            RuleFor(a => a.PostCode).NotEmpty();
        }
    }
}
