using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Dishes.Service.Business.Entities
{
    [ModelMetadataType(typeof(CookEntryDto))]
    public class CookEntryDto
    {
        public string Id { get; set; }
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
        public IEnumerable<CookEntryOptionHeaderDto> OptionHeaders { get; set; }
    }
}
