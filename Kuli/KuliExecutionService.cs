using System;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Core;

namespace Kuli
{
    public class KuliExecutionService : IHostedService
    {
        private readonly KuliExportPipeline _exportPipeline;

        private readonly KuliImportPipeline _importPipeline;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly ILogger<KuliExecutionService> _logger;
        private readonly LoggingLevelSwitch _loggingLevelSwitch;
        private readonly LoggingOptions _loggingOptions;

        public KuliExecutionService(
            ILogger<KuliExecutionService> logger,
            IOptions<LoggingOptions> loggingOptions,
            LoggingLevelSwitch loggingLevelSwitch,
            IHostApplicationLifetime lifetime,
            KuliImportPipeline importPipeline,
            KuliExportPipeline exportPipeline)
        {
            _logger = logger;
            _loggingOptions = loggingOptions.Value;
            _loggingLevelSwitch = loggingLevelSwitch;
            _lifetime = lifetime;
            _importPipeline = importPipeline;
            _exportPipeline = exportPipeline;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _loggingLevelSwitch.MinimumLevel = _loggingOptions.Level;

                await _importPipeline.RunAsync(cancellationToken);
                await _exportPipeline.RunAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogCritical("Encountered an unrecoverable error, exiting.\n{ex}", ex);
            }
            finally
            {
                _lifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}