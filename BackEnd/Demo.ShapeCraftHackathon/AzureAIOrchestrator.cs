using Microsoft.Extensions.Logging;
using ShapeCraft.AzureAISearch.Services.Contracts;
using ShapeCraft.MessageQueue.Services.Contracts;
using ShapeCraft.Storage.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ShapeCraftHackathon
{
    public class AzureAIOrchestrator
    {
        private readonly ILogger<AzureAIOrchestrator> _logger;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IBlobService _blobService;
        private readonly IIngestionService _ingestService;
        private readonly IRagChatService _ragChatService;

        public AzureAIOrchestrator(ILogger<AzureAIOrchestrator> logger, IMessageQueueService messageQueueService, IBlobService blobService, IIngestionService ingestionService, IRagChatService ragChatService)
        {
            _logger = logger;
            _messageQueueService = messageQueueService;
            _blobService = blobService;
            _ingestService = ingestionService;
            _ragChatService = ragChatService;
        }


        public async Task RunAsync()
        {
            _logger.LogInformation("🚀 Starting Azure AI orchestration");

            while (true)
            {
                var message = await _messageQueueService.ReceiveQueueMessageAsync();
                if (message == null)
                    break;

                var content = await _blobService.DownloadBlobContentAsync(message.BlobUrl);
                await _ingestService.IngestAsync(message.BlobUrl, content);

                _logger.LogInformation("Succesfully processed blob with url" + message.BlobUrl);
            }

            _logger.LogInformation("No more messages in queue, stopping.");


            string answer2 = await _ragChatService.AskAsync("What are the fees when I am buying an azuki?");
            Console.WriteLine(answer2);

        }

    }
}