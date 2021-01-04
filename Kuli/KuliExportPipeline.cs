using System.Threading;
using System.Threading.Tasks;
using Kuli.Rendering;

namespace Kuli
{
    public class KuliExportPipeline
    {
        private readonly StaticContentExportService _assetExporter;
        private readonly FragmentExportService _fragmentExporter;

        public KuliExportPipeline(FragmentExportService fragmentExporter, StaticContentExportService assetExporter)
        {
            _fragmentExporter = fragmentExporter;
            _assetExporter = assetExporter;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await _fragmentExporter.ExportFragmentsAsync(cancellationToken);
            await _assetExporter.ExportStaticContentAsync(cancellationToken);
        }
    }
}