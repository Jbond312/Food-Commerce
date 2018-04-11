namespace Dishes.Service.Business.Entities
{
    public class CookEntryOptionDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        /// <summary>
        /// Relates to an existing CookEntry. No other data required if property is set.
        /// </summary>
        public string CookEntryId { get; set; }
    }
}
