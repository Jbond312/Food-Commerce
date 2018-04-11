using Common.Repository;
using Common.Services;
using Common.Validation;
using Dishes.Service.Business.Entities;
using Dishes.Service.Business.Validators;
using Foods.Service.Repository.Dishes;
using Microsoft.Extensions.DependencyInjection;

namespace Dishes.Service.Business
{
    public class CookEntryServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<CookEntry>>(s => new Repository<CookEntry>(connectionString, databaseName, "dishes"));
            services.AddSingleton<IEntityValidator<CookEntryDto>, CookEntryValidator>();
        }
    }
}
