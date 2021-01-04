using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Kuli.Rendering
{
    public class SiteRenderer
    {
        private readonly ILogger<SiteRenderer> _logger;
        private readonly FragmentRenderer _renderer;
        private readonly RenderResultWriter _writer;
        private readonly GlobalContextService _globalContext;

        public SiteRenderer(ILogger<SiteRenderer> logger, FragmentRenderer renderer, RenderResultWriter writer,
            GlobalContextService globalContext)
        {
            _logger = logger;
            _renderer = renderer;
            _writer = writer;
            _globalContext = globalContext;
        }

        public async Task RenderFragmentsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Rendering fragments...");
            var sw = Stopwatch.StartNew();
            foreach (var fragmentEntry in _globalContext.Fragments)
            {
                var result = await _renderer.RenderFragmentAsync(fragmentEntry.Value);
                _writer.WriteResult(fragmentEntry.Value, result);
            }
            sw.Stop();
            _logger.LogInformation("Rendered {count} fragments in {time}ms", _globalContext.Fragments.Count, sw.ElapsedMilliseconds);
        }
    }
}