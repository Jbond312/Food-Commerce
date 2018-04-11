namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class ApplicantBusinessDto
    {
        public bool IsRegisteredWithCompaniesHouse { get; set; }
        public bool IsBusinessRegisteredOutsideUk { get; set; }
        public string CommercialRegister { get; set; }
        public string RegistrationNumber { get; set; }
        public string BusinessName { get; set; }
        public string VatNumber { get; set; }
        public string LegalStatus { get; set; }
        public string PositionInBusiness { get; set; }
        public string HomeCountry { get; set; }
        public AddressDto BusinessAddress { get; set; }
    }
}
