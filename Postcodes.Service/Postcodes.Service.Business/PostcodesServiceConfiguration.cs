using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Postcodes.Service.Business.Helper;

namespace Postcodes.Service.Business
{
    public class PostcodesServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton(typeof(PostcodesClient));
        }
    }
}
