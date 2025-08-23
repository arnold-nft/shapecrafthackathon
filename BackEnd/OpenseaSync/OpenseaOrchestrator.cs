
using ShapeCraft.Core.Options;
using ShapeCraft.OpenseaSync.Services.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;

namespace ShapeCraft.OpenseaSync
{
    public class OpenSeaOrchestrator
    {
        private readonly OpenSeaOptions _options;
        private readonly ILogger<OpenSeaOrchestrator> _logger;
        private readonly IOpenseaService _openSeaService;

        public OpenSeaOrchestrator(OpenSeaOptions options, ILogger<OpenSeaOrchestrator> logger, IOpenseaService openSeaService)
        {
            _options = options;
            _logger = logger;
            _openSeaService = openSeaService;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Starting OpenSea orchestration with BaseUrl: {BaseUrl}", _options.BaseUrl);

            var collections = new[]
            {
                "boredapeyachtclub",
                "cryptopunks",
                "azuki"
            };

            foreach (var slug in collections)
            {
                var openseaCollectionItem = await _openSeaService.GetCollectionAsync(slug);
                _logger.LogInformation("Fetched collection {Slug}:", slug);

            }

            _logger.LogInformation("OpenSea orchestration finished!");
        }
    }
}