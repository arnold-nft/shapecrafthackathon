
using Microsoft.Extensions.Logging;
using ShapeCraft.TransientLabsSync.Data;
using System.Threading.Tasks;

namespace ShapeCraft.TransientLabsSync
{
    public class TransientLabsOrchestrator
    {
        private readonly ILogger<TransientLabsOrchestrator> _logger;

        public TransientLabsOrchestrator(ILogger<TransientLabsOrchestrator> logger)
        {
            _logger = logger;
        }

        public async Task RunAsync()
        {
            _logger.LogInformation("Starting Transient Labs content ingestion simulation...");

            foreach (var content in TransientLabsTrainingData.SmallContentChunks)
            {
                _logger.LogInformation("Processing small Transient Labs content: {Content}", content);
            }

            foreach (var content in TransientLabsTrainingData.BigContentChunks)
            {
                _logger.LogInformation("Processing big Transient Labs content: {Content}", content);
            }

            _logger.LogInformation("Finished ingesting Transient Labs data.");
        }
    }
}
