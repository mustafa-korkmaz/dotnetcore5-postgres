using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using dotnetpostgres.Services;
using dotnetpostgres.Services.Account;
using dotnetpostgres.Services.Caching;
using dotnetpostgres.Services.Customer;
using dotnetpostgres.Services.Email;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace dotnetpostgres.tests
{
    public class TestStartup
    {
        public static readonly InMemoryDatabaseRoot InMemoryDatabaseRoot = new InMemoryDatabaseRoot();

        public TestStartup()
        {
          
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = GetApplicationConfiguration();

            services.AddDbContext<Dal.Databases.Postgres.PostgresDbContext>(
                opt =>
                    opt.UseInMemoryDatabase("UnitTestDb", InMemoryDatabaseRoot)
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

            services.AddTransient<Dal.IUnitOfWork, Dal.UnitOfWork>();

            // Add functionality to inject IOptions<T>
            services.AddOptions();

            // Add our Config object so it can be injected
            services.Configure<Configuration>(configuration.GetSection("Keys"));

            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<ICacheService, CacheService>();

            services.AddAutoMapper(typeof(MappingProfile));
        }

        public void Configure(IApplicationBuilder app)
        {
            // your usual registrations there
        }

        private IConfiguration GetApplicationConfiguration()
        {
            return new ConfigurationBuilder()
                .AddJsonFile(GetApplicationPath("settings.json"))
                .Build();
        }
        private string GetApplicationPath(string fileName)
        {
            var exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            Regex appPathMatcher = new Regex(@"(?<!fil)[A-Za-z]:\\+[\S\s]*?(?=\\+bin)");
            var appRoot = appPathMatcher.Match(exePath).Value;
            return Path.Combine(appRoot, fileName);
        }
    }
}