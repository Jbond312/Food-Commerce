using System.IO;
using System.Linq;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Users.Service.Api.SwaggerHelpers;

namespace Users.Service.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; set; }

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
                //x.SwaggerDoc("v2.0", new Info
                //{
                //    Title = "Users Service",
                //    Version = "v2.0",
                //    Description = "API responsible for user related queries and commands"
                //});
                //x.SwaggerDoc("v1.1", new Info
                //{
                //    Title = "Users Service",
                //    Version = "v1.1",
                //    Description = "API responsible for user related queries and commands"
                //});
                x.SwaggerDoc("v1.0", new Info
                {
                    Title = "Users Service",
                    Version = "v1.0",
                    Description = "API responsible for user related queries and commands"
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("default");

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
                {
                    //Order is important as it determines which API version is show first on the swagger page
                    //x.SwaggerEndpoint("/swagger/v2.0/swagger.json", "Users Service v2.0");
                    //x.SwaggerEndpoint("/swagger/v1.1/swagger.json", "Users Service v1.1");
                    x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Users Service v1.0");
                }
            );
            app.UseMvc();
        }
    }
}
