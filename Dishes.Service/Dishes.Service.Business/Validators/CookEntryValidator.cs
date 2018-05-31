using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Repository;
using Common.Validation;
using Dishes.Service.Business.Entities;
using FluentValidation;
using Foods.Service.Repository.Dishes;
using MongoDB.Driver;
using SystemSetting.Library.Api;

namespace Dishes.Service.Business.Validators
{
    public class CookEntryValidator : EntityValidator<CookEntryDto>
    {
        private readonly IRepository<CookEntry> _repo;
        private readonly SystemSettingsApi _systemSettingIntercom;

        public CookEntryValidator(IRepository<CookEntry> repo)
        {
            _repo = repo;
            _systemSettingIntercom = new SystemSettingsApi();

            CascadeMode = CascadeMode.StopOnFirstFailure;

            When(x => !string.IsNullOrEmpty(x.Id), () =>
            {
                RuleFor(x => x.Id).MustAsync(CookEntryMustExist);
                CreateCommonRules();
            });

            When(x => string.IsNullOrEmpty(x.Id), CreateCommonRules);
        }

        private void CreateCommonRules()
        {
            RuleFor(d => d.Name).NotEmpty().Length(1, 128).MustAsync(NameNotAlreadyExists).WithMessage("A dish already exists with this name.");
            RuleFor(d => d.Description).NotEmpty().Length(1, 256);
            RuleFor(d => d.Price).GreaterThan(0);
            RuleFor(d => d.Images).NotEmpty();
            RuleFor(d => d.Images == null ? 0 : d.Images.Count()).InclusiveBetween(0, 3).WithMessage("There must be only between 0 and 3 images per entry.");
            RuleFor(d => d.MenuHeading).NotEmpty().Length(1, 128);
            RuleFor(d => d.Ingredients).Length(0, 512);
            RuleFor(d => d.OptionHeaders).Must(OptionsMustBeUnique).WithMessage("All options must use unique names.").SetCollectionValidator(x => new CookEntryOptionHeaderValidator(_repo, x.CookId));
            RuleFor(d => d)
                .Cascade(CascadeMode.StopOnFirstFailure)
                .MustAsync(BeValidAllergens).WithMessage("Allergens must be unique and only match those within system settings")
                .MustAsync(BeValidDietPrefs).WithMessage("Dietary Preferences must be unique and only match those within system settings")
                .MustAsync(NotContradictAllergenAndDietaryPref).WithMessage("The given dietary preferences must not contradict with the given allergens for this dish.");
        }


        private async Task<bool> NotContradictAllergenAndDietaryPref(CookEntryDto cookEntry, CancellationToken token)
        {
            if (!cookEntry.Allergens.Any() && !cookEntry.DietaryPreferences.Any()) return true;

            var systemSettings = _systemSettingIntercom.ApiSystemSettingsGet().FirstOrDefault();

            foreach (var dietPref in cookEntry.DietaryPreferences)
            {
                var allergenBlackList = systemSettings.DietaryPreferences.First(x => x.Name == dietPref).ContradictiveAllergen.ToList();

                if (cookEntry.Allergens.Any(allergen => allergenBlackList.Contains(allergen)))
                {
                    return false;
                }
            }
            return true;
        }

        private async Task<bool> BeValidAllergens(CookEntryDto entry, CancellationToken token)
        {
            if (entry.Allergens == null)
            {
                return true;
            }

            var systemSettings = _systemSettingIntercom.ApiSystemSettingsGet().FirstOrDefault();

            return AllSettingsAreValid(entry.Allergens.ToList(), systemSettings?.Allergens?.ToList());
        }

        private async Task<bool> BeValidDietPrefs(CookEntryDto entry, CancellationToken token)
        {
            if (entry.DietaryPreferences == null)
            {
                return true;
            }
            var systemSettings = _systemSettingIntercom.ApiSystemSettingsGet().FirstOrDefault();

            return AllSettingsAreValid(entry.DietaryPreferences.ToList(), systemSettings?.DietaryPreferences?.Select(x => x.Name).ToList());
        }

        private bool AllSettingsAreValid(ICollection<string> cookEntryData, ICollection<string> systemSettings)
        {
            if (cookEntryData == null || systemSettings == null || !systemSettings.Any())
            {
                return true;
            }

            if (cookEntryData.Distinct(StringComparer.OrdinalIgnoreCase).Count() != cookEntryData.Count)
            {
                return false;
            }

            var invalidAllergens = cookEntryData.Except(systemSettings, StringComparer.OrdinalIgnoreCase);

            return !invalidAllergens.Any();
        }

        private async Task<bool> CookEntryMustExist(CookEntryDto cookEntryDto, string cookEntryId, CancellationToken token)
        {
            return await _repo.Exists(cookEntryId);
        }

        private bool OptionsMustBeUnique(CookEntryDto cookEntryDto, IEnumerable<CookEntryOptionHeaderDto> optionHeaders)
        {
            var optionHeadersList = optionHeaders.ToList();
            var uniqueHeaders = optionHeadersList.Select(x => x.Name).Distinct();

            var allOptions = optionHeadersList.SelectMany(x => x.Options).Where(x => x.Name != null).ToList();
            var uniqueOptions = allOptions.Select(x => x.Name).Distinct();

            return uniqueHeaders.Count() == optionHeadersList.Count && uniqueOptions.Count() == allOptions.Count;
        }

        private async Task<bool> NameNotAlreadyExists(CookEntryDto cookEntryDto, string name, CancellationToken token)
        {
            var filterBuilder = new FilterDefinitionBuilder<CookEntry>();
            var filter = filterBuilder.Where(x => !x.Id.Equals(cookEntryDto.Id) && x.CookId.Equals(cookEntryDto.CookId) && x.Name.Equals(cookEntryDto.Name));
            var dishes = await _repo.GetAll(filter);

            return !dishes.Any();
        }
    }
}