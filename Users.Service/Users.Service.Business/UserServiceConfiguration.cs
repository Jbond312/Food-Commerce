using Common.Services;
using Common.Validation;
using Foods.Service.Repository.Users;
using Foods.Service.Repository.Users.Entities;
using Microsoft.Extensions.DependencyInjection;
using Users.Service.Business.Entities;
using Users.Service.Business.Validators;

namespace Users.Service.Business
{
    public class UserServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IUserRepository<User>>(s => new UserRepository(connectionString, databaseName, "users"));

            services.AddSingleton<IEntityValidator<UserDto>, UserValidator>();
        }
    }
}
