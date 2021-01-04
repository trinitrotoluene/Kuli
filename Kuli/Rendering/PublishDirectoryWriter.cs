using System.IO;
using System.Text;
using System.Threading.Tasks;
using Kuli.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Rendering
{
    public class PublishDirectoryWriter
    {
        private readonly DirectoryDiscoveryOptions _dirOptions;
        private readonly ILogger<PublishDirectoryWriter> _logger;

        public PublishDirectoryWriter(IOptions<DirectoryDiscoveryOptions> dirOptions,
            ILogger<PublishDirectoryWriter> logger)
        {
            _logger = logger;
            _dirOptions = dirOptions.Value;
        }

        public Task WriteTextFileAsync(string relativePath, string content, string extension,
            Encoding encoding = default)
        {
            return WriteBinaryFileAsync(relativePath, (encoding ?? Encoding.UTF8).GetBytes(content), extension);
        }

        public Task WriteBinaryFileAsync(string relativePath, byte[] content, string extension)
        {
            var filePath = Path.ChangeExtension(Path.Combine(_dirOptions.Output, relativePath), extension);
            _logger.LogTrace("Writing output file to {filePath}", filePath);

            Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
            return File.WriteAllBytesAsync(filePath, content);
        }
    }
}