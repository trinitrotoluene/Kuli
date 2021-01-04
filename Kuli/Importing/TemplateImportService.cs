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
        private readonly ILogger<TemplateImportService> _logger;
        private readonly DirectoryOptions _options;
        private readonly SiteRenderingContext _siteRenderingContext;

        public TemplateImportService(IOptions<DirectoryOptions> options, ILogger<TemplateImportService> logger,
            SiteRenderingContext siteContext)
        {
            _logger = logger;
            _siteRenderingContext = siteContext;
            _options = options.Value;
        }

        public async Task ImportTemplatesAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            var basePath = Path.GetFullPath(_options.Templates);
            _logger.LogInformation("Importing templates in {path}", basePath);

            var files = Directory.GetFiles(basePath, "*.liquid", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                _logger.LogDebug("Importing {file}", file);
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                if (FluidTemplate.TryParse(content, out var template))
                {
                    var templateName = Path.GetFileNameWithoutExtension(file);
                    _siteRenderingContext.Templates[templateName] = template;
                }
                else
                {
                    _logger.LogWarning("Failed to parse template {file}", file);
                }
            }

            sw.Stop();
            _logger.LogInformation("Imported {count} templates in {time}ms", _siteRenderingContext.Templates.Count,
                sw.ElapsedMilliseconds);
        }
    }
}