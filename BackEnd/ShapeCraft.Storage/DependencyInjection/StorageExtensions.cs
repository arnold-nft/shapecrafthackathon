using ShapeCraft.Storage.Services;
using ShapeCraft.Storage.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapeCraft.Core.Options;
using AzureIndexer.Storage.Services;

namespace ShapeCraft.Storage.DependencyInjection
{
    public static class StorageExtensions
    {
        public static IServiceCollection AddStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddConfiguration<BlobStorageOptions>(options =>
               {
                   configuration.GetSection("StorageOptions").Bind(options);
               });


            services.AddSingleton<IBlobService, BlobService>();

            return services;
        }
    }
}
