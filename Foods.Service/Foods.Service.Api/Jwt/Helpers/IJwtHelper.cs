namespace Foods.Service.Api.Jwt.Helpers
{
    public interface IJwtHelper
    {
        string GenerateToken(FoodsIdentityUser user);
    }
}
