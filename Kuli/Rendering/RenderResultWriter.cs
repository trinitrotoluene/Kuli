using System.IO;
using System.Text;
using Kuli.Configuration;
using Kuli.Importing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Rendering
{
    public class RenderResultWriter
    {
        private readonly ILogger<RenderResultWriter> _logger;
        private readonly DirectoryDiscoveryOptions _directoryOptions;

        public RenderResultWriter(ILogger<RenderResultWriter> logger, IOptions<DirectoryDiscoveryOptions> directoryOptions)
        {
            _logger = logger;
            _directoryOptions = directoryOptions.Value;
        }

        public void WriteResult(Fragment fragment, string result)
        {
            Directory.CreateDirectory(_directoryOptions.Output);
            var path = Path.Combine(_directoryOptions.Output, fragment.Name + ".html");
            _logger.LogDebug("Create: {name} -> {path}", fragment.Name, path);
            
            File.WriteAllTextAsync(path + ".html", result, Encoding.UTF8);
        }
    }
}