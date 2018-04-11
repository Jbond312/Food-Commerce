namespace Foods.Service.Repository.Cooks.FoodBusiness
{
    public class ApplicantBusiness
    {
        //Applicant Business
        public bool IsRegisteredWithCompaniesHouse { get; set; }
        //If IsRegisteredWithCompaniesHouse is FALSE
        public bool IsBusinessRegisteredOutsideUk { get; set; }
        //IF IsBusinessRegisteredOutsideUK is TRUE
        public string CommercialRegister { get; set; }
        //If IsRegisteredWithCompaniesHouse is TRUE OR IsBusinessRegisteredOutsideUK is TRUE
        public string RegistrationNumber { get; set; }

        public string BusinessName { get; set; }
        public string VatNumber { get; set; }
        public string LegalStatus { get; set; }
        public string PositionInBusiness { get; set; }
        public string HomeCountry { get; set; }

        //Registered Address
        //Country where HQ of business is located or Address with companies house
        public Address BusinessAddress { get; set; }
    }
}
