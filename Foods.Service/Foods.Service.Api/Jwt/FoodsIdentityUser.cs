using Microsoft.AspNetCore.Identity;

namespace Foods.Service.Api.Jwt
{
    public class FoodsIdentityUser : IdentityUser
    {
        public string UserId { get; set; }
        public string CookId { get; set; }
        public string PreviousEmail { get; set; }
    }
}
