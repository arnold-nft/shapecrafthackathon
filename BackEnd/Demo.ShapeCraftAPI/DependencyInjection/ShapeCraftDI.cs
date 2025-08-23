using ShapeCraft.ManifoldSync;
using ShapeCraft.OpenseaSync;
using ShapeCraft.OpenseaSync.DependencyInjection;
using ShapeCraft.TransientLabsSync;
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

            services.AddOpenSea(configuration);




            return services;
        }
    }
}
