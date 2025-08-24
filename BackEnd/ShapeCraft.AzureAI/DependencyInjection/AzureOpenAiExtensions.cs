using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using ShapeCraft.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeCraft.AzureAI.Services.Contracts;

namespace ShapeCraft.AzureAI.DependencyInjection
{
    public static class AzureOpenAIOptions
    {

        public static IServiceCollection AddAzureOpenAI(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddConfiguration<AzureAIOptions>(options =>
               {
                   configuration.GetSection("AzureAIOptions").Bind(options);
               });


            services.AddSingleton<IEmbeddingService, EmbeddingService>();

            return services;
        }
    }
}
