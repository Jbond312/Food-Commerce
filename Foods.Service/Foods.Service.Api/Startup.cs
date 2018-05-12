using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using AutoMapper;
using Foods.Service.Api.FilterAttributes.Audit;
using Foods.Service.Api.FilterAttributes.JwtInterceptor;
using Foods.Service.Api.Jwt;
using Foods.Service.Api.Middleware.Extensions;
using Foods.Service.Api.SwaggerHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Swagger;

namespace Foods.Service.Api
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
            
            /*
             * It is very important that the order of these method calls is not changed!
             */

            WaitForDbInit();
            // ===== Add our DbContext ========
            services.AddDbContext<ApplicationDbContext>(ops => { ops.UseMySql(GetMySqlConnectionString()); }
            );

            // ===== Add Identity ========
            services.AddIdentity<FoodsIdentityUser, IdentityRole>(ops =>
                {
                    ops.Password.RequiredLength = 7;
                    ops.Password.RequireDigit = true;
                    ops.Password.RequireLowercase = true;
                    ops.Password.RequireUppercase = true;
                    ops.Password.RequireNonAlphanumeric = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear(); // => remove default claims
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["Jwt:JwtIssuer"],
                        ValidAudience = Configuration["Jwt:JwtIssuer"],
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:JwtKey"])),
                        //ClockSkew = TimeSpan.Zero // remove delay of token when expire
                    };
                });

            services.AddMvc()
                .AddMvcOptions(opt => opt.Filters.Add<AuditAttribute>())
                .AddMvcOptions(opt => opt.Filters.Add<JwtInterceptorAttribute>());

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:4200", "http://localhost:64611")
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
                x.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "Please insert JWT with Bearer.",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                x.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[] { } }
                });

                x.SwaggerDoc("v1.0", new Info
                {
                    Title = "Foods Service",
                    Version = "v1.0",
                    Description = "Aggregation of all internal foods services"
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
                .AddJsonFile("appsettings.json")
                .AddJsonFile("RateLimiting.json");

            Configuration = builder.Build();

            var mongoConnectionString = Configuration["MongoDatabase:ConnectionString"];
            var mongodatabaseName = Configuration["MongoDatabase:DatabaseName"];

            var serviceConfiguration = new ServiceConfigurator(services, mongoConnectionString, mongodatabaseName);
            serviceConfiguration.ConfigureServices();

            services.AddAutoMapper();

            services.AddMemoryCache();

            services.Configure<RateLimiting>(Configuration.GetSection("RateLimiting"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ApplicationDbContext dbContext)
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
                    x.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Foods Service v1.0");
                }
            );

            app.UseRateLimitingMiddleware();

            //app.UseAuditMiddleware();

            app.UseMvc();

            Console.WriteLine("Applying migrations");
            dbContext.Database.Migrate();
        }

        private string GetMySqlConnectionString()
        {
            var server = Configuration["IdentityDatabase:Server"];
            var port = Configuration["IdentityDatabase:Port"];
            var database = Configuration["IdentityDatabase:Database"];
            var userId = Configuration["IdentityDatabase:UserId"];
            var password = Configuration["IdentityDatabase:Password"];

            return $"Server={server};" +
                   $"Port={port};" +
                   $"database={database};" +
                   $"uid={userId};" +
                   $"pwd={password};" +
                   $"pooling=false;";
        }

        private void WaitForDbInit()
        {

            var retries = 1;
            while (retries < 7)
            {
                try
                {
                    using (var mySqlCon = new MySqlConnection(GetMySqlConnectionString()))
                    {
                        Console.WriteLine("Connecting to db. Trial: {0}", retries);
                        mySqlCon.Open();
                        mySqlCon.Close();
                        Console.WriteLine("Connecting to db succeded. Trial: {0}", retries);
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connecting to db failed. MySql Exception. {0}", ex.Message);

                    if (retries == 1)
                    {
                        Thread.Sleep(15000); //If it failed on the first try. It's like a fresh rollout
                    }
                    else
                    {
                        Thread.Sleep((int) Math.Pow(2, retries) * 1000);
                    }

                    retries++;
                }
            }
        }
    }
}