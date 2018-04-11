using Audit.Service.Business.Validators;
using Audit.Service.Business.Validators.Models;
using Common.Repository;
using Common.Services;
using Common.Validation;
using Foods.Service.Repository.Audit;
using Microsoft.Extensions.DependencyInjection;


namespace Audit.Service.Business
{
    public class AuditServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<AuditRecord>>(s => new Repository<AuditRecord>(connectionString, databaseName, "audit"));

            services.AddSingleton<IEntityValidator<Entities.AuditRecordDto>, AuditRecordValidator>();
            services.AddSingleton<IEntityValidator<AuditLogValidatorModel>, AuditLogValidator>();
        }
    }
}
