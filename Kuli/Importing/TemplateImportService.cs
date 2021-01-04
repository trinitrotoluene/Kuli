using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Fluid;
using Kuli.Configuration;
using Kuli.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Importing
{
    public class TemplateImportService
    {
        private readonly DirectoryDiscoveryOptions _options;
        private readonly ILogger<TemplateImportService> _logger;
        private readonly GlobalContextService _globalContext;

        public TemplateImportService(IOptions<DirectoryDiscoveryOptions> options, ILogger<TemplateImportService> logger,
            GlobalContextService globalContext)
        {
            _logger = logger;
            _globalContext = globalContext;
            _options = options.Value;
        }

        public async Task ImportTemplatesAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for templates in {directory}", _options.Templates);
            var sw = Stopwatch.StartNew();
            
            var files = Directory.GetFiles(_options.Templates, "*.liquid", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                _logger.LogDebug("Importing {file}", file);
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                if (FluidTemplate.TryParse(content, out var template))
                {
                    var templateName = Path.GetFileNameWithoutExtension(file);
                    _globalContext.Templates[templateName] = template;
                }
                else
                {
                    _logger.LogWarning("Failed to parse template {file}", file);
                }
            }
            
            sw.Stop();
            _logger.LogInformation("Imported {count} templates in {time}ms", _globalContext.Templates.Count, sw.ElapsedMilliseconds);
        }
    }
}