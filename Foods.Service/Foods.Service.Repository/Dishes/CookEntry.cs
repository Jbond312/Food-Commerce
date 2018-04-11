using System.Collections.Generic;
using Common.Repository;

namespace Foods.Service.Repository.Dishes
{
    public class CookEntry : BaseEntity
    {
        public string CookId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public IEnumerable<string> Images { get; set; }
        public string MenuHeading { get; set; }
        public bool IsActive { get; set; }
        public string Ingredients { get; set; }
        public IEnumerable<string> Allergens { get; set; }
        public IEnumerable<string> DietaryPreferences { get; set; }
        public IEnumerable<CookEntryOptionHeader> OptionHeaders { get; set; } 
    }
}
