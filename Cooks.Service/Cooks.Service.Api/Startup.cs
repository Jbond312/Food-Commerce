using System.IO;
using System.Linq;
using AutoMapper;
using Cooks.Service.Api.SwaggerHelpers;
using Cooks.Service.Business;
using Dishes.Service.Business;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Postcodes.Service.Business;
using Swashbuckle.AspNetCore.Swagger;

namespace Cooks.Service.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set;  }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation();

            services.AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = "https://localhost:44318";
                    options.RequireHttpsMetadata = true;
                    options.ApiName = "user.api";
                });

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddApiVersioning(o =>
            {
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new MediaTypeApiVersionReader();
            });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1.0", new Info
                {
                    Title = "Cooks Service",
                    Version = "v1.0",
                    Description = "API responsible for Cooks related queries and commands"
                });


                x.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();
                    // would mean this action is unversioned and should be included everywhere
                    if (actionApiVersionModel == null)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
                });
                x.OperationFilter<ApiVersionOperationFilter>();
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddAutoMapper();

            var serviceConfigurator = new CookServiceConfiguration();

            var mongoConnectionString = Configuration["MongoDatabase:ConnectionString"];
            var mongodatabaseName = Configuration["MongoDatabase:DatabaseName"];

            serviceConfigurator.ConfigureServices(services, mongoConnectionString, mongodatabaseName);

            var postcodeServiceConfigurator = new PostcodesServiceConfiguration();
            postcodeServiceConfigurator.ConfigureServices(services,"","");

            var dishServiceConfigurator = new CookEntryServiceConfiguration();
            dishServiceConfigurator.ConfigureServices(services,"","");
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
                {
                    //Order is important as it determines which API version is show first on the swagger page
                    x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Cooks Service v1.0");
                }
            );

            app.UseMvc();
        }
    }
}
