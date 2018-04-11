using Microsoft.Extensions.DependencyInjection;

namespace Common.Services
{
    public interface IServiceConfiguration
    {
        void ConfigureServices(IServiceCollection services, string connectionString, string databaseName);
    }
}
