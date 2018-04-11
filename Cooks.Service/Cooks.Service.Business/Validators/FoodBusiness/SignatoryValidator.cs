using System;
using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;

namespace Cooks.Service.Business.Validators.FoodBusiness
{
    public class SignatoryValidator : EntityValidator<SignatoryDto>
    {
        public SignatoryValidator()
        {
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                RuleFor(d => d.FullName).NotEmpty();
                RuleFor(d => d.Capacity).NotEmpty();
                RuleFor(d => d.Date).NotEmpty();
                RuleFor(d => d.Date).NotEmpty().Must(date => date <= DateTime.UtcNow)
                    .WithMessage("The signatory date must be today or earlier");
            });
        }
    }
}