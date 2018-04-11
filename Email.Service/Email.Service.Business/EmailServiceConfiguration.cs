using Common.Repository;
using Common.Services;
using Email.Service.Business.Helper;
using Foods.Service.Repository.Emails;
using Microsoft.Extensions.DependencyInjection;

namespace Email.Service.Business
{
    public class EmailServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<EmailTemplate>>(s => new Repository<EmailTemplate>(connectionString, databaseName, "emailTemplates"));

            services.AddSingleton(typeof(EmailHelper));
        }
    }
}
