using System.Collections.Generic;
using System.Diagnostics;
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
        private readonly DirectoryDiscoveryOptions _options;
        private readonly RawFragmentImporterService _importerService;
        private readonly ILogger<FragmentDiscoveryService> _logger;

        public FragmentDiscoveryService(IOptions<DirectoryDiscoveryOptions> options, RawFragmentImporterService importerService, ILogger<FragmentDiscoveryService> logger)
        {
            _importerService = importerService;
            _logger = logger;
            _options = options.Value;
        }

        public async Task<RawFragment[]> DiscoverFragmentsAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Searching for fragments in {directory}", _options.Fragments);

            var files = Directory.GetFiles(_options.Fragments, "*.md", SearchOption.AllDirectories);
            var fragments = new LinkedList<RawFragment>();
            
            foreach (var file in files)
            {
                _logger.LogDebug("Importing {file}", file);
                var content = await File.ReadAllTextAsync(file, cancellationToken);
                var rawFragment = _importerService.Import(Path.GetFileNameWithoutExtension(file), content);
                fragments.AddLast(rawFragment);
            }
            
            _logger.LogInformation("Discovered {count} fragments", fragments.Count);
            return fragments.ToArray();
        }
    }
}