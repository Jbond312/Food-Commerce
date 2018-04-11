using System.Collections.Generic;

namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class DeclarationDto
    {
        public bool IsUnderstood { get; set; }
        public List<SignatoryDto> Signatories { get; set; }
    }
}
