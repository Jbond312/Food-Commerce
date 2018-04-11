using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class ApplicantBusinessValidator : EntityValidator<ApplicantBusinessDto>
    {
        private readonly IEntityValidator<AddressDto> _addressValidator;

        public ApplicantBusinessValidator(IEntityValidator<AddressDto> addressValidator)
        {
            _addressValidator = addressValidator;

            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                When(ab => ab.IsRegisteredWithCompaniesHouse, () =>
                {
                    RuleFor(ab => ab.RegistrationNumber).NotEmpty();
                });

                When(ab => ab.IsBusinessRegisteredOutsideUk, () =>
                {
                    RuleFor(ab => ab.RegistrationNumber).NotEmpty();
                    RuleFor(ab => ab.CommercialRegister).NotEmpty();
                });

                RuleFor(ab => ab.RegistrationNumber).NotEmpty();
                RuleFor(ab => ab.BusinessName).NotEmpty();
                RuleFor(ab => ab.VatNumber).NotEmpty();
                RuleFor(ab => ab.LegalStatus).NotEmpty();
                RuleFor(ab => ab.PositionInBusiness).NotEmpty();
                RuleFor(ab => ab.HomeCountry).NotEmpty();
                RuleFor(ab => ab.BusinessAddress).SetValidator(_addressValidator.Validator);

                SetCommonRules();
            });
        }

        private void SetCommonRules()
        {
            When(ab => !ab.IsRegisteredWithCompaniesHouse, () =>
            {
                RuleFor(ab => ab.RegistrationNumber).Null();
            });

            When(ab => !ab.IsBusinessRegisteredOutsideUk, () =>
            {
                RuleFor(ab => ab.CommercialRegister).Null();
            });
        }
    }
}
