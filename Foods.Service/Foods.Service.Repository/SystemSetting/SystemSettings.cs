using System.Collections.Generic;
using Common.Repository;

namespace Foods.Service.Repository.SystemSetting
{
    public class SystemSettings : BaseEntity
    {
        public IEnumerable<string> Allergens { get; set; }
        public IEnumerable<DietaryPreference> DietaryPreferences { get; set; }
        public IEnumerable<string> CookCategories { get; set; }
    }

    public class DietaryPreference
    {
        public string Name { get; set; }
        public IEnumerable<string> ContradictiveAllergen { get; set; }
    }
}
