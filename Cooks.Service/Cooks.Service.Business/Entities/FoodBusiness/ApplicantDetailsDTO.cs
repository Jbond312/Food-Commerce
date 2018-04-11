namespace Cooks.Service.Business.Entities.FoodBusiness
{
    public class ApplicantDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MainTelephoneNumber { get; set; }
        public string OtherTelephoneNumber { get; set; }
        public bool CanContactViaTelephone { get; set; }
    }
}
