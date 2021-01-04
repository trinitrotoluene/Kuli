using System;
using Microsoft.Extensions.Logging;

namespace Kuli.Importing
{
    public sealed class RawFragmentImporterService
    {
        private readonly ILogger<RawFragmentImporterService> _logger;

        public RawFragmentImporterService(ILogger<RawFragmentImporterService> logger)
        {
            _logger = logger;
        }

        private const string FrontMatterFence = "---";

        public RawFragment Import(string fileName, string rawElement)
        {
            _logger.LogTrace("Importing raw fragment");
            var (fenceEndIndex, frontMatter) = ImportFrontMatter(rawElement);
            var pageMarkdown = ImportMarkdown(rawElement, fenceEndIndex);

            return new RawFragment(frontMatter, pageMarkdown, fileName);
        }

        private string ImportMarkdown(string rawFragment, int startIndex)
        {
            if (startIndex >= rawFragment.Length)
                return string.Empty;
            
            var markdownStartIndex = startIndex switch
            {
                0 => 0,
                _ => startIndex + FrontMatterFence.Length
            };

            _logger.LogTrace("Extracting markdown from index {start}", markdownStartIndex);
            return rawFragment.Substring(markdownStartIndex);
        }

        private (int, string) ImportFrontMatter(string rawElement)
        {
            var fenceStartIndex = rawElement.IndexOf(FrontMatterFence, 0, StringComparison.Ordinal);
            if (fenceStartIndex < 0)
                return (0, string.Empty);

            var fenceEndIndex = rawElement.IndexOf(FrontMatterFence, fenceStartIndex + FrontMatterFence.Length,
                StringComparison.Ordinal);
            
            if (fenceEndIndex < fenceStartIndex)
                throw new ImportException("Missing closing front matter tag.");

            var startIndex = fenceStartIndex + FrontMatterFence.Length;
            var length = fenceEndIndex - startIndex;

            _logger.LogTrace("Detected front matter in fragment, extracting {length} characters from index {start}", length, startIndex);
            return (fenceEndIndex, rawElement.Substring(startIndex, fenceEndIndex - startIndex));
        }
    }
}