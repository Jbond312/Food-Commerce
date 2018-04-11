using Common.Repository;
using Common.Services;
using Foods.Service.Repository.SystemSetting;
using Microsoft.Extensions.DependencyInjection;

namespace SystemSetting.Service.Business
{
    public class SystemSettingServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<SystemSettings>>(s => new Repository<SystemSettings>(connectionString, databaseName, "systemSettings"));
        }
    }
}
