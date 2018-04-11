namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class FoodBusinessEstablishmentDto
    {
        public string FoodBusinessName { get; set; }
        public bool IsSameAddress { get; set; }
        public AddressDto EstablishmentAddress { get; set; }
    }
}
