using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShapeCraft.ManifoldSync;
using ShapeCraft.OpenseaSync;
using ShapeCraft.OpenseaSync.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            services.ConfigureFunctionsApplicationInsights();

            services.AddOpenSea(configuration);

            services.AddTransient<OpenSeaOrchestrator>();

            services.AddTransient<ManifoldOrchestrator>();


            return services;
        }
    }
}
