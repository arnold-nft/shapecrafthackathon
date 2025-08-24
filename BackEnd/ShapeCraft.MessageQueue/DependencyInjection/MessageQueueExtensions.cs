using ShapeCraft.MessageQueue.Services;
using ShapeCraft.MessageQueue.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShapeCraft.Core.Options;

namespace ShapeCraft.MessageQueue.DependencyInjection
{
    public static class MessageQueueExtensions
    {
        public static IServiceCollection AddMessageQueues(this IServiceCollection services, IConfiguration configuration)
        {
            services
               .AddConfiguration<MessageQueueOptions>(options =>
               {
                   configuration.GetSection("MessageQueueOptions").Bind(options);
               });


            services.AddSingleton<IMessageQueueService, ServiceBusService>();

            return services;
        }

    }
}
