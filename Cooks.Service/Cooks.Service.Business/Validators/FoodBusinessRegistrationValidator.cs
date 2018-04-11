using System.Threading;
using System.Threading.Tasks;
using Common.Repository;
using Common.Validation;
using Cooks.Service.Business.Entities.FoodBusiness;
using Cooks.Service.Business.Validators.Models;
using FluentValidation;
using Foods.Service.Repository.Cooks;

namespace Cooks.Service.Business.Validators
{
    public class FoodBusinessRegistrationValidator : EntityValidator<FoodBusinessRegistrationValidatorModel>
    {
        private readonly IRepository<Cook> _cookRepo;
        private readonly IFoodBusinessRegistrationRepository<FoodBusinessRegistration> _foodBusRegRepo;
        private readonly IEntityValidator<ApplicantDetailsDto> _applicantDetailsValidator;
        private readonly IEntityValidator<ApplicantBusinessDto> _applicantBusinessValidator;
        private readonly IEntityValidator<FoodBusinessEstablishmentDto> _foodBusinessEstablishmentValidator;
        private readonly IEntityValidator<FoodBusinessOperatorDto> _foodBusinessOperatorValidator;
        private readonly IEntityValidator<BusinessOperationDto> _businessOperatorValidator;
        private readonly IEntityValidator<DeclarationDto> _declarationValidator;

        public FoodBusinessRegistrationValidator(
            IRepository<Cook> cookRepo,
            IFoodBusinessRegistrationRepository<FoodBusinessRegistration> foodBusRegRepo,
            IEntityValidator<ApplicantDetailsDto> applicantDetailsValidator,
            IEntityValidator<ApplicantBusinessDto> applicantBusinessValidator,
            IEntityValidator<FoodBusinessEstablishmentDto> foodBusinessEstablishmentValidator,
            IEntityValidator<FoodBusinessOperatorDto> foodBusinessOperatorValidator,
            IEntityValidator<BusinessOperationDto> businessOperatorValidator,
            IEntityValidator<DeclarationDto> declarationValidator
        )
        {
            _cookRepo = cookRepo;
            _foodBusRegRepo = foodBusRegRepo;
            _applicantDetailsValidator = applicantDetailsValidator;
            _applicantBusinessValidator = applicantBusinessValidator;
            _foodBusinessEstablishmentValidator = foodBusinessEstablishmentValidator;
            _foodBusinessOperatorValidator = foodBusinessOperatorValidator;
            _businessOperatorValidator = businessOperatorValidator;
            _declarationValidator = declarationValidator;
            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            RuleSet(RuleSets.FoodBusinessRegistrationCompleted.ToString(), () =>
            {
                SetCommonRules();

                RuleFor(fr => fr.FoodBusinessRegistration.ApplicantDetails).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.ApplicantBusiness).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.FoodBusinessEstablishment).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.FoodBusinessOperator).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.TypeOfFoodBusiness).NotEmpty();
                RuleFor(fr => fr.FoodBusinessRegistration.BusinessOperation).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.NumberOfPeopleInFoodBusiness).NotEmpty();
                RuleFor(fr => fr.FoodBusinessRegistration.Declaration).NotNull();
                RuleFor(fr => fr.FoodBusinessRegistration.TypeOfFoodBusiness).NotEmpty();

            });

            RuleSet(RuleSets.FoodBusinessRegistrationNotCompleted.ToString(), () =>
            {
                SetCommonRules();
            });


        }

        private void SetCommonRules()
        {
            RuleFor(fr => fr.FoodBusinessRegistration.CookId).Cascade(CascadeMode.StopOnFirstFailure).MustAsync(CookExists).WithMessage("The cook does not exist.");

            When(fr => string.IsNullOrEmpty(fr.FoodBusinessRegistration.Id), () =>
            {
                RuleFor(fr => fr.FoodBusinessRegistration.CookId).Cascade(CascadeMode.StopOnFirstFailure).MustAsync(HaveNoRegistration).WithMessage("The cook already has an existing registration.");
            });

            When(fr => !string.IsNullOrEmpty(fr.FoodBusinessRegistration.Id), () =>
            {
                RuleFor(fr => fr.FoodBusinessRegistration.CookId).Cascade(CascadeMode.StopOnFirstFailure).MustAsync(HaveARegistration).WithMessage("The cook has no associated registration.");
                RuleFor(fr => fr.FoodBusinessRegistration.CookId).Cascade(CascadeMode.StopOnFirstFailure).MustAsync(MustHaveMatchingFoodRegId).WithMessage("The food registration id does not belong to this cook.");
            });   

            RuleFor(fr => fr.FoodBusinessRegistration.ApplicantDetails).SetValidator(_applicantDetailsValidator.Validator);
            RuleFor(fr => fr.FoodBusinessRegistration.ApplicantBusiness).SetValidator(_applicantBusinessValidator.Validator);
            RuleFor(fr => fr.FoodBusinessRegistration.FoodBusinessEstablishment).SetValidator(_foodBusinessEstablishmentValidator.Validator);
            RuleFor(fr => fr.FoodBusinessRegistration.FoodBusinessOperator).SetValidator(_foodBusinessOperatorValidator.Validator);
            RuleFor(fr => fr.FoodBusinessRegistration.BusinessOperation).SetValidator(_businessOperatorValidator.Validator);
            RuleFor(fr => fr.FoodBusinessRegistration.Declaration).SetValidator(_declarationValidator.Validator);
        }

        private async Task<bool> MustHaveMatchingFoodRegId(FoodBusinessRegistrationValidatorModel model, string cookId, CancellationToken cancellationToken)
        {
            var foodBusReg = await _foodBusRegRepo.Get(model.FoodBusinessRegistration.Id);
            return foodBusReg?.CookId == cookId;
        }

        private async Task<bool> HaveARegistration(string cookId, CancellationToken cancellationToken)
        {
            var foodBusRegId = await GetCooksFoodBusinessRegistrationId(cookId);
            return !string.IsNullOrEmpty(foodBusRegId);
        }

        private async Task<bool> HaveNoRegistration(string cookId, CancellationToken cancellationToken)
        {
            var foodBusRegId = await GetCooksFoodBusinessRegistrationId(cookId);
            return string.IsNullOrEmpty(foodBusRegId);
        }

        private async Task<string> GetCooksFoodBusinessRegistrationId(string cookId)
        {
            var foodBusReg = await _foodBusRegRepo.GetByCookId(cookId);
            return foodBusReg?.Id;
        }

        private async Task<bool> CookExists(string cookId, CancellationToken cancellationToken)
        {
            return await _cookRepo.Exists(cookId);
        }
    }   
}
