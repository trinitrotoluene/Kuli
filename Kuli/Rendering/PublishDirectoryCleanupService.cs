using System.IO;
using Kuli.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Rendering
{
    public class PublishDirectoryCleanupService
    {
        private readonly ILogger<PublishDirectoryCleanupService> _logger;
        private readonly DirectoryOptions _dirOptions;

        public PublishDirectoryCleanupService(ILogger<PublishDirectoryCleanupService> logger, IOptions<DirectoryOptions> dirOptions)
        {
            _logger = logger;
            _dirOptions = dirOptions.Value;
        }

        public void CleanOutputDirectory()
        {
            var outputDir = Path.GetFullPath(_dirOptions.Output);
            _logger.LogDebug("Cleaning output directory {dir}", outputDir);

            foreach (var file in Directory.EnumerateFiles(outputDir, "*", SearchOption.AllDirectories))
            {
                _logger.LogTrace("Deleting {file}", file);
                File.Delete(file);
            }
        }
    }
}