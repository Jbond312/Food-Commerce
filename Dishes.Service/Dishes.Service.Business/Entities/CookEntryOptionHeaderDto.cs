using System.Collections.Generic;

namespace Dishes.Service.Business.Entities
{
    public class CookEntryOptionHeaderDto
    {
        public string Name { get; set; }
        public OptionTypes OptionType { get; set; }
        public IEnumerable<CookEntryOptionDto> Options { get; set; }
    }
}
