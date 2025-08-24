using ShapeCraft.AzureAI.DependencyInjection;
using ShapeCraft.AzureAISearch.DependencyInjection;
using ShapeCraft.ManifoldSync;
using ShapeCraft.MessageQueue.DependencyInjection;
using ShapeCraft.OpenseaSync;
using ShapeCraft.OpenseaSync.DependencyInjection;
using ShapeCraft.Storage.DependencyInjection;
using ShapeCraft.TransientLabsSync;

namespace Demo.ShapeCraftHackathon.DependencyInjection
{
    public static class ShapeCraftDI
    {

        public static IServiceCollection AddShapeCraftDI(
           this IServiceCollection services,
           IConfiguration configuration
       )
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddOpenSea(configuration);

            services.AddTransient<OpenSeaOrchestrator>();

            services.AddTransient<ManifoldOrchestrator>();

            services.AddTransient<TransientLabsOrchestrator>();

            services.AddMessageQueues(configuration);

            services.AddStorage(configuration);

            services.AddAzureSearch(configuration);

            services.AddAzureOpenAI(configuration);

            return services;
        }
    }
}
