using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using ShapeCraft.AzureAI;
using ShapeCraft.ManifoldSync;
using ShapeCraft.OpenseaSync;
using ShapeCraft.TransientLabsSync;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.ShapeCraftHackathon
{
    public class MainOrchestrator
    {

        private readonly OpenSeaOrchestrator _openSeaOrchestrator;
        private readonly ManifoldOrchestrator _manifoldOrchestrator;
        private readonly TransientLabsOrchestrator _transientLabsOrchestrator;
        private readonly AzureAIOrchestrator _azureAIOrchestrator;

        public MainOrchestrator(OpenSeaOrchestrator openSeaOrchestrator, ManifoldOrchestrator manifoldOrchestrator, TransientLabsOrchestrator transientLabsOrchestrator, AzureAIOrchestrator azureAIOrchestrator)
        {
            _openSeaOrchestrator = openSeaOrchestrator;
            _manifoldOrchestrator = manifoldOrchestrator;
            _transientLabsOrchestrator = transientLabsOrchestrator;
            _azureAIOrchestrator = azureAIOrchestrator;
        }

        [Function(nameof(MainOrchestrator))]
        public async Task RunAsync(
            [TimerTrigger("0 0 * * * *", RunOnStartup = true)]
            FunctionContext context)
        {
            var logger = context.GetLogger(nameof(MainOrchestrator));
            logger.LogInformation("MainOrchestrator function triggered locally");

            await _openSeaOrchestrator.RunAsync();

            await _manifoldOrchestrator.RunAsync();

            await _transientLabsOrchestrator.RunAsync();

            await _azureAIOrchestrator.RunAsync();

            logger.LogInformation("MainOrchestrator executed successfully!");
        }
    }
}
