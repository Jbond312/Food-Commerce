using System.Collections.Generic;

namespace SystemSetting.Service.Business.Entities
{
    public class SystemSettingsDto
    {
        public IEnumerable<string> Allergens { get; set; }
        public IEnumerable<DietaryPreferenceDto> DietaryPreferences { get; set; }
        public IEnumerable<string> CookCategories { get; set; }
    }

    public class DietaryPreferenceDto
    {
        public string Name { get; set; }
        public IEnumerable<string> ContradictiveAllergen { get; set; }
    }
}
