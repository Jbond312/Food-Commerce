using System.Collections.Generic;

namespace Foods.Service.Repository.Cooks.FoodBusiness
{
    public class Declaration
    {
        public bool IsUnderstood { get; set; }
        public List<Signatory> Signatories { get; set; }
    }
}
