using Common.Repository;
using Common.Services;
using Foods.Service.Repository.TextContent;
using Microsoft.Extensions.DependencyInjection;

namespace TextContent.Service.Business
{
    public class TextContentServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<TextContents>>(s => new Repository<TextContents>(connectionString, databaseName, "textContents"));
        }
    }
}