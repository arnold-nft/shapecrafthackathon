using Microsoft.Extensions.DependencyInjection;
using ShapeCraft.AzureAISearch.Services;
using ShapeCraft.AzureAISearch.Services.Contracts;
using ShapeCraft.Core.Options;
using Microsoft.Extensions.Configuration;

namespace ShapeCraft.AzureAISearch.DependencyInjection
{
    public static class AzureSearchAIOptions
    {

        public static IServiceCollection AddAzureSearch(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddConfiguration<AzureSearchOptions>(options =>
               {
                   configuration.GetSection("SearchAIOptions").Bind(options);
               });


            services.AddSingleton<IVectorSearchService, VectorSearchService>();

            services.AddSingleton<IRagChatService, RagChatService>();

            services.AddSingleton<IIngestionService, IngestionService>();

            return services;
        }
    }
}
