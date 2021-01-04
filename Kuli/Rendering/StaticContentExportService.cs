using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Configuration;
using Kuli.Importing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Rendering
{
    public class StaticContentExportService
    {
        private readonly DirectoryDiscoveryOptions _dirOptions;
        private readonly StaticContentImportService _importService;
        private readonly ILogger<StaticContentExportService> _logger;
        private readonly PublishDirectoryWriter _outputWriter;
        private readonly SiteRenderingContext _siteContext;

        public StaticContentExportService(ILogger<StaticContentExportService> logger,
            PublishDirectoryWriter outputWriter,
            SiteRenderingContext siteContext, StaticContentImportService importService,
            IOptions<DirectoryDiscoveryOptions> dirOptions)
        {
            _logger = logger;
            _outputWriter = outputWriter;
            _siteContext = siteContext;
            _importService = importService;
            _dirOptions = dirOptions.Value;
        }

        public async Task ExportStaticContentAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Writing static assets to output directory");

            foreach (var assetEntry in _siteContext.StaticContent)
            {
                var asset = assetEntry.Value;
                var assetOutputPath = Path.Combine(_dirOptions.AssetsOutput, asset.SubDir, asset.FileName);
                await _outputWriter.WriteBinaryFileAsync(assetOutputPath, asset.Value, asset.Extension);
            }
        }
    }
}