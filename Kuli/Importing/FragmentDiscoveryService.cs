using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kuli.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Kuli.Importing
{
    public class FragmentDiscoveryService
    {
        private readonly RawFragmentImporterService _importerService;
        private readonly ILogger<FragmentDiscoveryService> _logger;
        private readonly DirectoryOptions _options;

        public FragmentDiscoveryService(IOptions<DirectoryOptions> options,
            RawFragmentImporterService importerService, ILogger<FragmentDiscoveryService> logger)
        {
            _importerService = importerService;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<RawFragment[]> DiscoverFragmentsAsync(CancellationToken cancellationToken)
        {
            var basePath = Path.GetFullPath(_options.Fragments);
            _logger.LogInformation("Importing fragments in {path}", basePath);

            var files = Directory.GetFiles(basePath, "*.md", SearchOption.AllDirectories);
            var fragments = new LinkedList<RawFragment>();

            foreach (var file in files)
            {
                _logger.LogDebug("Importing {file}", basePath);
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                var rawFragment = _importerService.Import(Path.GetFileNameWithoutExtension(file), content);
                fragments.AddLast(rawFragment);
            }

            _logger.LogInformation("Discovered {count} fragments", fragments.Count);
            return fragments.ToArray();
        }
    }
}