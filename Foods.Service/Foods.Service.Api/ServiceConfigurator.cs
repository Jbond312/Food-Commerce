using System.Collections.Generic;
using Audit.Service.Business;
using Common.Services;
using Common.Validation;
using Microsoft.Extensions.DependencyInjection;
using Users.Service.Business;
using Cooks.Service.Business;
using Dishes.Service.Business;
using Foods.Service.Api.Jwt.Helpers;
using Email.Service.Business;
using Foods.Service.Api.Jwt.Entities;
using Foods.Service.Api.Jwt.Entities.Validators;
using Foods.Service.Intercom.Audit;
using Foods.Service.Intercom.Email;
using Foods.Service.Intercom.Postcode;
using Foods.Service.Intercom.SystemSetting;
using Postcodes.Service.Business;

namespace Foods.Service.Api
{
    public class ServiceConfigurator
    {
        private readonly IServiceCollection _services;
        private readonly string _connectionString;
        private readonly string _databaseName;

        public ServiceConfigurator(IServiceCollection services, string connectionString, string databaseName)
        {
            _services = services;
            _connectionString = connectionString;
            _databaseName = databaseName;
        }

        private static IEnumerable<IServiceConfiguration> Services => new List<IServiceConfiguration>
        {
            new UserServiceConfiguration(),
            new CookServiceConfiguration(),
            new AuditServiceConfiguration(),
            new CookEntryServiceConfiguration(),
            new EmailServiceConfiguration(),
            new PostcodesServiceConfiguration()
        };

        public void ConfigureServices()
        {
            foreach (var service in Services)
            {
                service.ConfigureServices(_services, _connectionString, _databaseName);
            }

            _services.AddSingleton<IAuditIntercom, AuditIntercom>();
            _services.AddSingleton<IJwtHelper, JwtHelper>();
            _services.AddSingleton<IEntityValidator<RegisterDto>, RegisterValidator>();
            _services.AddSingleton<IEntityValidator<LoginDto>, LoginValidator>();

            //Must be transient as constructor takes in UserManager<FoodsIdentityUser>
            _services.AddTransient<IAccountHelper, AccountHelper>();
            _services.AddSingleton<IEmailIntercom, EmailIntercom>();
            _services.AddSingleton<ISystemSettingIntercom, SystemSettingIntercom>();
            _services.AddSingleton<IPostcodeIntercom, PostcodeIntercom>();
        }
    }
}
