using System.IO;
using System.Linq;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TextContent.Service.Api.SwaggerHelpers;
using TextContent.Service.Business;
using AutoMapper;

namespace TextContent.Service.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().AddFluentValidation();

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
                    Title = "System Settings Service",
                    Version = "v1.0",
                    Description = "API responsible for System Setting related queries and commands"
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
                x.DescribeAllEnumsAsStrings();
            });

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            services.AddAutoMapper();

            var serviceConfigurator = new TextContentServiceConfiguration();

            var mongoConnectionString = Configuration["MongoDatabase:ConnectionString"];
            var mongodatabaseName = Configuration["MongoDatabase:DatabaseName"];

            serviceConfigurator.ConfigureServices(services, mongoConnectionString, mongodatabaseName);
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
                    x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Text Content Service v1.0");
                }
            );

            app.UseMvc();
        }
    }
}
