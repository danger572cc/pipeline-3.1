using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WIS.Database.Setup.Core;
using WIS.Database.Setup.Core.Services;
using WIS.Database.Setup.Imp;
using WIS.Database.Setup.Models;

namespace WIS.Database.Setup
{
    public sealed class Startup
    {
        public readonly IWMSVersionService _versionManager;

        public readonly ILogger<Startup> _logger;

        public Startup(IWMSVersionService versionManager, ILogger<Startup> logger)
        {
            _logger = logger;
            _versionManager = versionManager;
        }

        public async Task Run(string[] args)
        {
            _versionManager.GetDataSource();
            var versions = _versionManager.GetVersionsToMigrate();
            /*var pipeline = new PipelineBuilder<List<WMSReleaseInfo>>()
                .Register(new ScriptsExecutionStep(_versionManager, _logger, 1))
                .Register(new FakeScriptsExecutionStep(_versionManager, _logger, 2))
                .Register(new ScriptsExecutionStep(_versionManager, _logger, 3))
                .Build();
            await pipeline.Execute(versions);*/
            /*var pipelineBuilder = new PipelineBuilder<List<WMSReleaseInfo>>();
            for (int step = 0; step < versions.Count ; step++)
            {
                pipelineBuilder.Register(new ScriptsExecutionStep(_versionManager, _logger, step));
            }
            var pipeline = pipelineBuilder.Build();
            await pipeline.Execute(versions);*/
        }
    }
}
