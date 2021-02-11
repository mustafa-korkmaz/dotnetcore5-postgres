using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Console;
using Npgsql.Logging;

namespace dotnetpostgres.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
#if DEBUG
            NpgsqlLogManager.Provider = new ConsoleLoggingProvider(NpgsqlLogLevel.Debug);
            NpgsqlLogManager.IsParameterLoggingEnabled = true;
#endif
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                 .ConfigureLogging((hostingContext, logging) =>
                 {
                     logging.ClearProviders();
#if DEBUG
                     logging.AddSimpleConsole(options => options.IncludeScopes = true);
#endif
                     //logging.AddDebug();
                 })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
