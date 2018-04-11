using System;
using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class BusinessOperationValidator : EntityValidator<BusinessOperationDto>
    {
        public BusinessOperationValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                When(bo => bo.IsNewBusiness, () =>
                {
                    RuleFor(bo => bo.OpeningDate).NotEmpty();
                    RuleFor(bo => bo.OpeningDate).NotEmpty().Must(openDate => openDate > DateTime.UtcNow)
                        .WithMessage("The opening date must be greater than todays date.");
                });

                When(bo => bo.IsSeasonalBusiness, () =>
                {
                    RuleFor(bo => bo).NotNull();
                    RuleFor(bo => bo).NotNull()
                        .Must(bo => bo.SeasonStart >= DateTime.UtcNow && bo.SeasonStart < bo.SeasonEnd)
                        .WithMessage("The start date must be greater than today and less than the end date.");
                    RuleFor(bo => bo).NotNull();
                    RuleFor(bo => bo).NotNull()
                        .Must(bo => bo.SeasonEnd >= DateTime.UtcNow && bo.SeasonEnd > bo.SeasonStart)
                        .WithMessage("The end date must be greater than today and greater than the start date.");
                });

                SetCommonValidationRules();
            });
        }

        private void SetCommonValidationRules()
        {
            When(bo => !bo.IsNewBusiness, () =>
            {
                RuleFor(bo => bo.OpeningDate).Null();
            });

            When(bo => !bo.IsSeasonalBusiness, () =>
            {
                RuleFor(bo => bo.SeasonStart).Null();
                RuleFor(bo => bo.SeasonEnd).Null();
            });
        }
    }
}
