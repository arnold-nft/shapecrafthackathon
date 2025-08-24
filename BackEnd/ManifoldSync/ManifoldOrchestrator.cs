using Microsoft.Extensions.Logging;
using ShapeCraft.ManifoldSync.Data;
using ShapeCraft.MessageQueue.Models;
using ShapeCraft.MessageQueue.Services.Contracts;
using ShapeCraft.Storage.Services.Contracts;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace ShapeCraft.ManifoldSync
{
    public class ManifoldOrchestrator
    {

        /*
         * IMPROVEMENT OPPORTUNITY:
         * 
         * Currently, Manifold.xyz's Connect Widget allows users to authenticate with their wallets 
         * and access NFT data programmatically through a client-side data client. 
         * Embedding this widget in a frontend could enable your AI chatbot to fetch real-time,
         * user-specific NFT information dynamically.
         * 
         * However, this approach relies on each user connecting their wallet, meaning the AI 
         * would only have access to the authenticated user's data. This limits bulk or global 
         * dataset building unless many users upload their data actively.
         * 
         * Despite this limitation, enabling user-driven data queries can be valuable for 
         * generative experiences, such as asking the AI questions about specific wallet 
         * addresses, NFTs owned, or collectors associated with a contract.
         * 
         * NOTE: For this hackathon, integrating the Connect Widget is likely out of scope, 
         * but it represents a significant potential improvement that could be added later 
         * to enhance real-time and personalized data retrieval.
         */

        private readonly ILogger<ManifoldOrchestrator> _logger;
        private readonly IBlobService _blobService;
        private readonly IMessageQueueService _messageQueueService;

        public ManifoldOrchestrator(ILogger<ManifoldOrchestrator> logger, IMessageQueueService messageQueueService, IBlobService blobService)
        {
            _logger = logger;
            _blobService = blobService;
            _messageQueueService = messageQueueService;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Starting Manifold content ingestion simulation...");

            foreach (var content in ManifoldTrainingData.SmallContentChunks)
            {
                _logger.LogInformation("Processing small manifold content");

                //storage explorer upload
                var uri = await _blobService.UploadItemAsync(content, $"manifold-{Guid.NewGuid()}");

                _logger.LogInformation("Put message on Service bus");
                //put message on bus
                var message = new QueueMessage(uri.ToString(), DateTime.UtcNow);
                await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(message));
            }

            foreach (var content in ManifoldTrainingData.BigContentChunks)
            {
                _logger.LogInformation("Processing big manifold content");

                //storage explorer upload
                var uri = await _blobService.UploadItemAsync(content, $"manifold-{Guid.NewGuid()}");

                _logger.LogInformation("Put message on Service bus");
                //put message on bus
                var message = new QueueMessage(uri.ToString(), DateTime.UtcNow);
                await _messageQueueService.SendMessageAsync(JsonSerializer.Serialize(message));
            }

            _logger.LogInformation("Finished ingesting Manifold data.");
        }
    }
}
