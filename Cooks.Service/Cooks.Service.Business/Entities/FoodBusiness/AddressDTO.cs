namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class AddressDto
    {
        public string HouseNumber { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public LocationDto Location { get; set; }
    }
}
