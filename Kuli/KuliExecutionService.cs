using System;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Configuration;
using Kuli.Importing;
using Kuli.Rendering;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog.Core;

namespace Kuli
{
    public class KuliExecutionService : IHostedService
    {
        private readonly ILogger<KuliExecutionService> _logger;
        private readonly LoggingOptions _loggingOptions;
        private readonly LoggingLevelSwitch _loggingLevelSwitch;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly FragmentImportService _fragmentService;
        private readonly TemplateImportService _templateService;
        private readonly SiteRenderer _renderService;

        public KuliExecutionService(
            ILogger<KuliExecutionService> logger,
            IOptions<LoggingOptions> loggingOptions, 
            LoggingLevelSwitch loggingLevelSwitch, 
            IHostApplicationLifetime lifetime, 
            FragmentImportService fragmentService, 
            TemplateImportService templateService, 
            SiteRenderer renderService)
        {
            _logger = logger;
            _loggingOptions = loggingOptions.Value;
            _loggingLevelSwitch = loggingLevelSwitch;
            _lifetime = lifetime;
            _fragmentService = fragmentService;
            _templateService = templateService;
            _renderService = renderService;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _loggingLevelSwitch.MinimumLevel = _loggingOptions.Level;
                await StartInternalAsync(cancellationToken);
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

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        private async Task StartInternalAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Building context...");
            
            await _fragmentService.ImportFragmentsAsync(cancellationToken);
            await _templateService.ImportTemplatesAsync(cancellationToken);
            await _renderService.RenderFragmentsAsync(cancellationToken);
        }
    }
}