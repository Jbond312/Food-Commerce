using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Repository;
using Common.Validation;
using Cooks.Service.Business.Entities;
using Cooks.Service.Business.Entities.FoodBusiness;
using FluentValidation;
using Foods.Service.Intercom.SystemSetting;
using Foods.Service.Repository.Cooks;
using MongoDB.Driver;

namespace Cooks.Service.Business.Validators
{
    public class CookValidator : EntityValidator<CookDto>
    {
        private readonly IRepository<Cook> _cookRepo;
        private readonly ISystemSettingIntercom _systemSettingIntercom;
        private readonly IEntityValidator<AddressDto> _addressValidator;

        public CookValidator(
            IRepository<Cook> cookRepo,
            ISystemSettingIntercom systemSettingIntercom,
            IEntityValidator<AddressDto> addressValidator
            )
        {
            _cookRepo = cookRepo;
            _systemSettingIntercom = systemSettingIntercom;
            _addressValidator = addressValidator;

            CreateValidationRules();
        }

        private void CreateValidationRules()
        {
            When(cook => !string.IsNullOrEmpty(cook.Id),
                () =>
                {
                    RuleFor(ck => ck.Id).MustAsync(NotAlreadyExist).WithMessage("The cook does not exist.");
                });
            
            RuleFor(ck => ck.DisplayName).NotEmpty();
            RuleFor(ck => ck.DisplayName).MustAsync(HaveUniqueName).WithMessage("A cook already exists with this name.");
            RuleFor(ck => ck.Categories).NotEmpty().Must(MustHaveUniqueCategories).WithMessage("A cook must have between 1 and 3 unique categories.");
            RuleFor(ck => ck.Categories == null ? 0 : ck.Categories.Count).InclusiveBetween(1, 3).WithMessage("A cook must have between 1 and 3 categories.");
            RuleFor(d => d.Categories).MustAsync(BeValidCategories).WithMessage("The categories must be unique and only match those within system settings");

            RuleSet(RuleSets.Cooks.ToString(), () =>
            {
                RuleFor(ck => ck.Address).NotNull().SetValidator(_addressValidator.Validator);
            });
        }

        private async Task<bool> BeValidCategories(ICollection<string> categories, CancellationToken token)
        {
            var systemSettings = await _systemSettingIntercom.GetAllSystemSettings();

            if (categories == null)
            {
                return true;
            }

            if (categories.Distinct(StringComparer.OrdinalIgnoreCase).Count() != categories.Count)
            {
                return false;
            }

            var invalidAllergens = categories.Except(systemSettings.CookCategories, StringComparer.OrdinalIgnoreCase);

            return !invalidAllergens.Any();
        }

        private bool MustHaveUniqueCategories(CookDto cookDto, IEnumerable<string> categories)
        {
            var categoryList = categories.ToList();
            return categoryList.Distinct().Count() == categoryList.Count;
        }

        private async Task<bool> HaveUniqueName(CookDto cook, string displayName, CancellationToken token)
        {
            var filterBuilder = new FilterDefinitionBuilder<Cook>();
            var filter = filterBuilder.Where(x => x.DisplayName.Equals(displayName) && !x.Id.Equals(cook.Id));
            var displayNameExists = await _cookRepo.GetAll(filter);
            return !displayNameExists.Any();
        }

        private async Task<bool> NotAlreadyExist(string id, CancellationToken token)
        {
            var isExistingEntity = await _cookRepo.Exists(id);
            return isExistingEntity;
        }
    }
}