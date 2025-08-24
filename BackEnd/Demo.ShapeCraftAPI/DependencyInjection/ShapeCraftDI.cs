using ShapeCraft.AzureAI.DependencyInjection;
using ShapeCraft.AzureAISearch.DependencyInjection;

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

            services.AddAzureSearch(configuration);

            services.AddAzureOpenAI(configuration);

            return services;
        }
    }
}
