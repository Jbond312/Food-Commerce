using System.Collections.Generic;

namespace Foods.Service.Repository.Dishes
{
    public class CookEntryOptionHeader
    {
        public string Name { get; set; }
        public int OptionType { get; set; }
        public IEnumerable<CookEntryOption> Options { get; set; }
    }
}
