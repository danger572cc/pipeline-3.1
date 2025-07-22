using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using WIS.Database.Setup.Core.Services;
using WIS.Database.Setup.Core.Settings;
using WIS.Database.Setup.Imp.Services;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup.Core
{
    public static class Bootstrapper
    {
        public static void Bootstrap(this IServiceCollection services, IConfiguration configuration)
        {
            #region Settings
            var dbSettings = new DatabaseSettings();
            var setupSettings = new SetupSettings();
            var versionsSettings = new List<VersionsSettings>();
            configuration.GetSection("DatabaseSettings").Bind(dbSettings);
            configuration.GetSection("SetupSettings").Bind(setupSettings);
            configuration.GetSection("VersionsSettings").Bind(versionsSettings);
            services.AddSingleton(dbSettings);
            services.AddSingleton(setupSettings);
            services.AddSingleton(versionsSettings);
            #endregion
            // Services
            services.AddSingleton<IWMSVersionService, WMSVersionService>();
            //Memory Database
            services.AddSingleton<MemoryContext>();
            //NLog
            services.AddLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
                builder.AddNLog("nlog.config");
            });
        }
    }
}
