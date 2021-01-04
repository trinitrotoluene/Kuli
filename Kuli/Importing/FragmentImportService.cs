using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Rendering;
using Markdig;
using Microsoft.Extensions.Logging;
using YamlDotNet.Serialization;

namespace Kuli.Importing
{
    public class FragmentImportService
    {
        private readonly FragmentDiscoveryService _discoveryService;
        private readonly ILogger<FragmentImportService> _logger;
        private readonly MarkdownPipeline _markdownPipeline;
        private readonly SiteRenderingContext _siteRenderingContext;
        private readonly IDeserializer _yamlDeserializer;

        public FragmentImportService(ILogger<FragmentImportService> logger, IDeserializer yamlDeserializer,
            MarkdownPipeline markdownPipeline, FragmentDiscoveryService discoveryService,
            SiteRenderingContext siteContext)
        {
            _logger = logger;
            _yamlDeserializer = yamlDeserializer;
            _markdownPipeline = markdownPipeline;
            _discoveryService = discoveryService;
            _siteRenderingContext = siteContext;
        }

        public async Task ImportFragmentsAsync(CancellationToken cancellationToken)
        {
            var sw = Stopwatch.StartNew();
            var fragments = await _discoveryService.DiscoverFragmentsAsync(cancellationToken);

            _logger.LogInformation("Preprocessing fragments");

            foreach (var fragment in fragments)
            {
                var frontMatter = ProcessFrontMatter(fragment);
                _logger.LogTrace("YAML extraction result: {@FrontMatter}", frontMatter);
                var html = ProcessMarkdown(fragment);
                _logger.LogTrace("Markdown compilation result: {html}", html);

                var fragmentRef = frontMatter.TryGetValue("name", out var name) ? name : fragment.FileName;
                var processedFragment = new Fragment(fragmentRef, frontMatter, html);

                _siteRenderingContext.Fragments.Add(fragmentRef, processedFragment);
                _logger.LogDebug("Successfully imported fragment {name} to build context", fragmentRef);
            }

            sw.Stop();
            _logger.LogInformation("Imported {count} fragments in {time}ms", _siteRenderingContext.Fragments.Count,
                sw.ElapsedMilliseconds);
        }

        private string ProcessMarkdown(RawFragment fragment)
        {
            if (string.IsNullOrWhiteSpace(fragment.PageMarkdown))
            {
                _logger.LogTrace("Fragment has no markdown to process");
                return string.Empty;
            }

            var html = Markdown.ToHtml(fragment.PageMarkdown, _markdownPipeline);
            return html;
        }

        private Dictionary<string, string> ProcessFrontMatter(RawFragment fragment)
        {
            if (string.IsNullOrWhiteSpace(fragment.FrontMatter))
            {
                _logger.LogTrace("Fragment has no front matter to process");
                return new Dictionary<string, string>();
            }

            var frontMatter = _yamlDeserializer.Deserialize<Dictionary<string, string>>(fragment.FrontMatter);
            return frontMatter;
        }
    }
}