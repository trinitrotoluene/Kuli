using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kuli.Rendering
{
    public class FragmentExportService
    {
        private readonly ILogger<FragmentExportService> _logger;
        private readonly PublishDirectoryWriter _outputWriter;
        private readonly FragmentRenderer _renderer;
        private readonly SiteRenderingContext _siteRenderingContext;

        public FragmentExportService(ILogger<FragmentExportService> logger, FragmentRenderer renderer,
            SiteRenderingContext siteContext, PublishDirectoryWriter outputWriter)
        {
            _logger = logger;
            _renderer = renderer;
            _siteRenderingContext = siteContext;
            _outputWriter = outputWriter;
        }

        public async Task ExportFragmentsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rendering fragments...");
            var sw = Stopwatch.StartNew();
            foreach (var fragmentEntry in _siteRenderingContext.Fragments)
            {
                var fragment = fragmentEntry.Value;
                var result = await _renderer.RenderFragmentAsync(fragment);
                await _outputWriter.WriteTextFileAsync(fragment.Name, result, "html");
            }

            sw.Stop();
            _logger.LogInformation("Rendered {count} fragments in {time}ms", _siteRenderingContext.Fragments.Count,
                sw.ElapsedMilliseconds);
        }
    }
}