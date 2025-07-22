using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WIS.Database.Setup.Core;
using WIS.Database.Setup.Core.Services;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup.Imp
{
    public sealed class FakeScriptsExecutionStep : Step<List<WMSReleaseInfo>>
    {
        private readonly ILogger _logger;

        private readonly int _step;

        public readonly IWMSVersionService _versionManager;

        public FakeScriptsExecutionStep(IWMSVersionService versionManager, ILogger logger, int index)
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
                _logger.LogInformation($"(Fake)Entra paso {_step}.");
                throw new Exception("No funca!!!!!!!!!!!");
                _logger.LogInformation($"(Fake)Fin paso {_step}.");
                await next(context);
            }
            catch (Exception e) 
            {
                _logger.LogInformation($"Fin paso {_step} con error");
            }
        }
    }
}
