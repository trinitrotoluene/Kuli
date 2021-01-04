using System.Threading;
using System.Threading.Tasks;
using Kuli.Rendering;

namespace Kuli
{
    public class KuliExportPipeline
    {
        private readonly StaticContentExportService _assetExporter;
        private readonly FragmentExportService _fragmentExporter;
        private readonly PublishDirectoryCleanupService _cleanupService;

        public KuliExportPipeline(FragmentExportService fragmentExporter, StaticContentExportService assetExporter,
            PublishDirectoryCleanupService cleanupService)
        {
            _fragmentExporter = fragmentExporter;
            _assetExporter = assetExporter;
            _cleanupService = cleanupService;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            _cleanupService.CleanOutputDirectory();
            
            await _fragmentExporter.ExportFragmentsAsync(cancellationToken);
            await _assetExporter.ExportStaticContentAsync(cancellationToken);
        }
    }
}