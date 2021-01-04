using System.Threading.Tasks;
using Fluid;
using Kuli.Importing;
using Microsoft.Extensions.Logging;

namespace Kuli.Rendering
{
    public class FragmentRenderer
    {
        private readonly ILogger<FragmentRenderer> _logger;
        private readonly SiteRenderingContext _siteRenderingContext;

        public FragmentRenderer(ILogger<FragmentRenderer> logger, SiteRenderingContext siteContext)
        {
            _logger = logger;
            _siteRenderingContext = siteContext;
        }

        public async Task<string> RenderFragmentAsync(Fragment fragment)
        {
            if (!_siteRenderingContext.Templates.TryGetValue(fragment.Template, out var template))
                throw new RendererException(
                    $"Failed to resolve template {fragment.Template} for fragment {fragment.Name}.");

            var context = new TemplateContext();
            _siteRenderingContext.ApplyValues(context);

            context.SetValue(fragment.Type, fragment.Html);
            var renderResult = await template.RenderAsync(context);

            _logger.LogTrace("Render result: {result}", renderResult);

            return renderResult;
        }
    }
}