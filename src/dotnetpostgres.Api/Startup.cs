using System;
using System.Collections.Generic;
using dotnetpostgres.Services;
using dotnetpostgres.Services.Customer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using dotnetpostgres.Api.Middlewares;
using dotnetpostgres.Services.Account;
using dotnetpostgres.Services.Email;
using dotnetpostgres.Services.Parameter;
using dotnetpostgres.Services.User;
using Microsoft.Extensions.Logging;

namespace dotnetpostgres.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressModelStateInvalidFilter = true; //prevent automatic 400 response
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    };

                    options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                });

            services.AddDbContextPool<Dal.Databases.Postgres.PostgresDbContext>(opt =>
            {
                opt.UseNpgsql(Configuration.GetConnectionString("PostgresDbContext"));
            });

            services.AddTransient<Dal.IUnitOfWork, Dal.UnitOfWork>();

            //Injecting the identity manager
            services.AddIdentity<Dal.Entities.Identity.ApplicationUser,
                    Dal.Entities.Identity.ApplicationRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                })
                .AddEntityFrameworkStores<Dal.Databases.Postgres.PostgresDbContext>()
                .AddDefaultTokenProviders();

            // Add functionality to inject IOptions<T>
            services.AddOptions();
            // Add our Config object so it can be injected
            services.Configure<Configuration>(Configuration.GetSection("Keys"));

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IParameterService, ParameterService>();

            services.AddAutoMapper(typeof(MappingProfile));

            //todo: configure cors properly
            services.AddCors(config =>
            {
                var policy = new CorsPolicy();
                policy.Headers.Add("*");
                policy.Methods.Add("*");
                policy.Origins.Add("*");
                //policy.SupportsCredentials = true;
                config.AddPolicy("policy", policy);
            });

            services.ConfigureJwtAuthentication(Configuration);
            services.ConfigureJwtAuthorization();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "dotnetpostgres.Api", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "dotnetpostgres.Api v1"));

            app.UseHttpsRedirection();

            //request handling
            app.UseRequestMiddleware();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var logger = app.ApplicationServices.GetService<ILogger<Startup>>();

            logger.LogInformation($"application started working on {Environment.MachineName} at {DateTime.UtcNow:u}");
        }
    }
}
