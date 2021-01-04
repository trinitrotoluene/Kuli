using System.Threading;
using System.Threading.Tasks;
using Kuli.Importing;

namespace Kuli
{
    public class KuliImportPipeline
    {
        private readonly StaticContentImportService _assetsService;
        private readonly FragmentImportService _fragmentService;
        private readonly TemplateImportService _templateService;

        public KuliImportPipeline(FragmentImportService fragmentService,
            TemplateImportService templateService,
            StaticContentImportService assetsService)
        {
            _fragmentService = fragmentService;
            _templateService = templateService;
            _assetsService = assetsService;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            await _assetsService.ImportStaticContentAsync(cancellationToken);
            await _fragmentService.ImportFragmentsAsync(cancellationToken);
            await _templateService.ImportTemplatesAsync(cancellationToken);
        }
    }
}