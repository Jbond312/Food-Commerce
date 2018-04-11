namespace Foods.Service.Api.Jwt.Entities
{
    public class ConfirmEmailDto
    {
        public string EmailConfirmToken { get; set; }
        public string IdentityId { get; set; }
    }
}
