using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShapeCraft.OpenseaSync.Services.Contracts;
using ShapeCraft.OpenseaSync;
using ShapeCraft.Core.Options;

namespace ShapeCraft.OpenseaSync.DependencyInjection
{
    public static class OpenseaExtensions
    {

        public static IServiceCollection AddOpenSea(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddConfiguration<OpenSeaOptions>(options =>
            {
                configuration.GetSection("OpenSea").Bind(options);
            });

            services.AddHttpClient<IOpenseaService, OpenseaService>();

            return services;
        }
    }
}
