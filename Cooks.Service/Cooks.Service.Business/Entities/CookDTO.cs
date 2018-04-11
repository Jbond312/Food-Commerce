using System.Collections.Generic;
using Cooks.Service.Business.Entities.FoodBusiness;
using Microsoft.AspNetCore.Mvc;

namespace Cooks.Service.Business.Entities
{
    [ModelMetadataType(typeof(CookDto))]
    public class CookDto
    {
        public string Id { get; set; }
        public string FoodBusinessRegistrationId { get; set; }
        public string DisplayName { get; set; }
        public ICollection<string> Categories { get; set; }
        public AddressDto Address { get; set; }
    }
}
