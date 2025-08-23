using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace ShapeCraft.Core.Options
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection AddConfiguration<TOptions>(
            this IServiceCollection services,
            Action<TOptions> configureOptions
        ) where TOptions : class, new()
        {
            return services
                .Configure(configureOptions)
                .AddSingleton(provider => provider.GetRequiredService<IOptions<TOptions>>().Value);
        }
    }
}
