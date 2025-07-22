using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;
using WIS.Database.Setup.Core;

namespace WIS.Database.Setup
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
            var builder = new ConfigurationBuilder();
            BuildConfig(builder, environment);
            var host = Host.CreateDefaultBuilder()
                .UseEnvironment(environment)
                .ConfigureServices((context, services) =>
                {
                    // Servicios
                    services.Bootstrap(context.Configuration);
                })
                .Build();
            var startup = ActivatorUtilities.CreateInstance<Startup>(host.Services);
            await startup.Run(args);
            //NLog.LogManager.Shutdown();
        }

        static void BuildConfig(IConfigurationBuilder builder, string environment)
        {
            string appSettingsName;
            if (environment == "Production")
            {
                appSettingsName = "appsettings.json";
            }
            else
            {
                appSettingsName = $"appsettings.{environment}.json";
            }
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile(appSettingsName, optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
