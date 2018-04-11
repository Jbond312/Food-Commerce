using System.Collections.Generic;
using Common.Repository;
using Foods.Service.Repository.Cooks.FoodBusiness;

namespace Foods.Service.Repository.Cooks
{
    public class Cook : BaseEntity
    {
        public string DisplayName { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public Address Address { get; set; }
    }
}
