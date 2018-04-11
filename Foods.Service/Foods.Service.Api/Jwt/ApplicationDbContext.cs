using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Foods.Service.Api.Jwt
{
    public class ApplicationDbContext : IdentityDbContext<FoodsIdentityUser>
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(GetConnectionString());
        }

        private string GetConnectionString()
        {
            var server = _configuration["IdentityDatabase:Server"];
            var port = _configuration["IdentityDatabase:Port"];
            var database = _configuration["IdentityDatabase:Database"];
            var userId = _configuration["IdentityDatabase:UserId"];
            var password = _configuration["IdentityDatabase:Password"];

            return $"Server={server};" +
                   $"Port={port};" +
                   $"database={database};" +
                   $"uid={userId};" +
                   $"pwd={password};" +
                   $"pooling=false;";
        }
    }
}