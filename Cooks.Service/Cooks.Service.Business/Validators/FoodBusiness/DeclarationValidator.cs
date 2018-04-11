using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class DeclarationValidator : EntityValidator<DeclarationDto>
    {
        private readonly IEntityValidator<SignatoryDto> _signatoryValidator;

        public DeclarationValidator(IEntityValidator<SignatoryDto> signatoryValidator)
        {
            _signatoryValidator = signatoryValidator;

            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                RuleFor(d => d.IsUnderstood).Must(beTrue => beTrue);
                RuleFor(d => d.Signatories).NotEmpty();
                RuleForEach(d => d.Signatories).SetValidator(_signatoryValidator.Validator);
            });
        }
    }
}
