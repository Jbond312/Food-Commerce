using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class ApplicantDetailsValidator : EntityValidator<ApplicantDetailsDto>
    {
        public ApplicantDetailsValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                RuleFor(ad => ad.FirstName).NotEmpty();
                RuleFor(ad => ad.LastName).NotEmpty();
                RuleFor(ad => ad.MainTelephoneNumber).NotEmpty();
                RuleFor(ad => ad.OtherTelephoneNumber).NotEmpty();

                SetCommonValidationRules();
            });
        }

        private void SetCommonValidationRules()
        {
            When(ad => !string.IsNullOrEmpty(ad.Email), () =>
            {
                RuleFor(ad => ad.Email).NotEmpty().EmailAddress();
            });
        }
    }
}
