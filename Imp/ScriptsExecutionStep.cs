using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WIS.Database.Setup.Core;
using WIS.Database.Setup.Core.Services;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup.Imp
{
    public sealed class ScriptsExecutionStep : Step<List<WMSReleaseInfo>>
    {
        private readonly ILogger _logger;

        private readonly int _step;

        public readonly IWMSVersionService _versionManager;

        public ScriptsExecutionStep(IWMSVersionService versionManager, ILogger logger, int index)
        {
            _versionManager = versionManager;
            _logger = logger;
            _step = index;
        }

        protected override async Task Execute(List<WMSReleaseInfo> context, Func<List<WMSReleaseInfo>, Task> next)
        {
            //TODO: Ejecutar los scripts de versiones a la base y posterior actualizar la tabla de versiones
            try
            {
                var version = context.OrderBy(o => o.Name).ToList()[_step];
                _logger.LogInformation($"Entra paso {_step + 1}");
                _logger.LogInformation($"Ejecutando scripts del release {version.Name}");
                _logger.LogInformation($"Fin paso {_step + 1}");
                await next(context);
            }
            catch (Exception e) 
            {
                _logger.LogInformation($"Fin paso {_step} con error");
            }
        }
    }
}
