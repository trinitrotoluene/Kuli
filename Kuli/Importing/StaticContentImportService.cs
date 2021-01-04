using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Configuration;
using Kuli.Rendering;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Importing
{
    public class StaticContentImportService
    {
        private readonly DirectoryDiscoveryOptions _dirOptions;
        private readonly ILogger<StaticContentImportService> _logger;
        private readonly SiteRenderingContext _siteContext;

        public StaticContentImportService(ILogger<StaticContentImportService> logger,
            IOptions<DirectoryDiscoveryOptions> dirOptions, SiteRenderingContext siteContext)
        {
            _logger = logger;
            _siteContext = siteContext;
            _dirOptions = dirOptions.Value;
        }

        public async Task ImportStaticContentAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();

            var basePath = Path.GetFullPath(_dirOptions.Assets);
            _logger.LogInformation("Importing static assets in {path}", basePath);

            var files = Directory.EnumerateFiles(basePath, "*.*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(basePath, file);
                _logger.LogTrace("Importing asset {file} with relative path {relPath}", file, relativePath);

                var content = await File.ReadAllBytesAsync(file, cancellationToken);
                var staticContent = new StaticContent(relativePath, content);
                _siteContext.StaticContent[relativePath] = staticContent;

                _logger.LogTrace("Built asset {relPath}", staticContent.RelativePath);
            }

            sw.Stop();
            _logger.LogInformation("Imported {count} static assets in {time}ms", _siteContext.StaticContent.Count,
                sw.ElapsedMilliseconds);
        }
    }
}