
using AzureIndexer.Storage.Services;
using Microsoft.Extensions.Logging;
using ShapeCraft.MessageQueue.Models;
using ShapeCraft.MessageQueue.Services.Contracts;
using ShapeCraft.OpenseaSync.Services.Contracts;
using ShapeCraft.Storage.Services.Contracts;
using ShapeCraft.TransientLabsSync.Data;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeCraft.TransientLabsSync
{
    public class TransientLabsOrchestrator
    {
        private readonly ILogger<TransientLabsOrchestrator> _logger;

        private readonly IBlobService _blobService;
        private readonly IMessageQueueService _messageQueueService;

        public TransientLabsOrchestrator(ILogger<TransientLabsOrchestrator> logger, IMessageQueueService messageQueueService, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
            _messageQueueService = messageQueueService;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Starting Transient Labs content ingestion simulation...");

            foreach (var content in TransientLabsTrainingData.SmallContentChunks)
            {
                _logger.LogInformation("Processing small Transient Labs content: {Content}", content);

                //storage explorer upload
                var uri = await _blobService.UploadItemAsync(content, $"transientlabs-{Guid.NewGuid()}");

                _logger.LogInformation("Put message on Service bus");
                //put message on bus
                var message = new QueueMessage(uri.ToString(), DateTime.UtcNow);
                await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(message));
            }

            foreach (var content in TransientLabsTrainingData.BigContentChunks)
            {
                _logger.LogInformation("Processing big Transient Labs content: {Content}", content);

                //storage explorer upload
                var uri = await _blobService.UploadItemAsync(content, $"transientlabs-{Guid.NewGuid()}");

                _logger.LogInformation("Put message on Service bus");
                //put message on bus
                var message = new QueueMessage(uri.ToString(), DateTime.UtcNow);
                await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(message));
            }

            _logger.LogInformation("Finished ingesting Transient Labs data.");
        }
    }
}
