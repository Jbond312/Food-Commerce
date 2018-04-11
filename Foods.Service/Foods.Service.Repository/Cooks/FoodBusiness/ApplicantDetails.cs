namespace Foods.Service.Repository.Cooks.FoodBusiness
{
    public class ApplicantDetails
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //Must include country code
        public string MainTelephoneNumber { get; set; }
        public string OtherTelephoneNumber { get; set; }
        public bool CanContactViaTelephone { get; set; }
    }
}
