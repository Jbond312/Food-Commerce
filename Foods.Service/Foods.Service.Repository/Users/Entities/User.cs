using Common.Repository;

namespace Foods.Service.Repository.Users.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string IdentityProviderId { get; set; }
    }
}
