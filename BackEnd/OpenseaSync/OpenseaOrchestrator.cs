
using ShapeCraft.Core.Options;
using ShapeCraft.OpenseaSync.Services.Contracts;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Text.Json;
using ShapeCraft.Storage.Services.Contracts;
using ShapeCraft.MessageQueue.Services.Contracts;
using ShapeCraft.MessageQueue.Models;

namespace ShapeCraft.OpenseaSync
{
    public class OpenSeaOrchestrator
    {
        private readonly OpenSeaOptions _options;
        private readonly ILogger<OpenSeaOrchestrator> _logger;
        private readonly IOpenseaService _openSeaService;
        private readonly IBlobService _blobService;
        private readonly IMessageQueueService _messageQueueService;

        public OpenSeaOrchestrator(OpenSeaOptions options, ILogger<OpenSeaOrchestrator> logger, IOpenseaService openSeaService, IBlobService blobService, IMessageQueueService messageQueueService)
        {
            _options = options;
            _logger = logger;
            _openSeaService = openSeaService;
            _blobService = blobService;
            _messageQueueService = messageQueueService;
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

                _logger.LogInformation("Upload data to blob storage");
                //storage explorer upload
                var uri = await _blobService.UploadItemAsync(openseaCollectionItem, $"opensea-{slug}-collection");

                _logger.LogInformation("Put message on Service bus");
                //put message on bus
                var message = new QueueMessage(uri.ToString(), DateTime.UtcNow);
                await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(message));

            }

            _logger.LogInformation("OpenSea orchestration finished!");
        }
    }
}