using Cooks.Service.Business.Entities;

namespace Cooks.Service.Business.Validators.Models
{
    public class FoodBusinessRegistrationValidatorModel
    {
        public FoodBusinessRegistrationDto FoodBusinessRegistration { get; set; }

        public string RuleSet
        {
            get
            {
                if (FoodBusinessRegistration == null)
                    return string.Empty;

                return FoodBusinessRegistration.IsCompleted
                    ? RuleSets.FoodBusinessRegistrationCompleted.ToString()
                    : RuleSets.FoodBusinessRegistrationNotCompleted.ToString();
            }
        }
    }
}
