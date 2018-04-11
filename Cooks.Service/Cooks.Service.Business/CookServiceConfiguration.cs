using Common.Repository;
using Common.Services;
using Common.Validation;
using Cooks.Service.Business.Entities;
using Cooks.Service.Business.Entities.FoodBusiness;
using Cooks.Service.Business.Helpers;
using Cooks.Service.Business.Validators;
using Cooks.Service.Business.Validators.FoodBusiness;
using Cooks.Service.Business.Validators.Models;
using Foods.Service.Repository.Cooks;
using Microsoft.Extensions.DependencyInjection;

namespace Cooks.Service.Business
{
    public class CookServiceConfiguration : IServiceConfiguration
    {
        public void ConfigureServices(IServiceCollection services, string connectionString, string databaseName)
        {
            services.AddSingleton<IRepository<Cook>>(s => new Repository<Cook>(connectionString, databaseName, "cooks"));
            services.AddSingleton<IFoodBusinessRegistrationRepository<FoodBusinessRegistration>>(s => new FoodBusinessRegistrationRepository(connectionString, databaseName, "foodBusinessRegistrations"));

            services.AddSingleton<ILocationHelper, LocationHelper>();

            services.AddSingleton<IEntityValidator<CookDto>, CookValidator>();
            services.AddSingleton<IEntityValidator<FoodBusinessRegistrationValidatorModel>, FoodBusinessRegistrationValidator>();
            services.AddSingleton<IEntityValidator<AddressDto>, AddressValidator>();
            services.AddSingleton<IEntityValidator<ApplicantBusinessDto>, ApplicantBusinessValidator>();
            services.AddSingleton<IEntityValidator<ApplicantDetailsDto>, ApplicantDetailsValidator>();
            services.AddSingleton<IEntityValidator<BusinessOperationDto>, BusinessOperationValidator>();
            services.AddSingleton<IEntityValidator<DeclarationDto>, DeclarationValidator>();
            services.AddSingleton<IEntityValidator<FoodBusinessEstablishmentDto>, FoodBusinessEstablishmentValidator>();
            services.AddSingleton<IEntityValidator<FoodBusinessOperatorDto>, FoodBusinessOperatorValidator>();
            services.AddSingleton<IEntityValidator<SignatoryDto>, SignatoryValidator>();
        }
    }
}
