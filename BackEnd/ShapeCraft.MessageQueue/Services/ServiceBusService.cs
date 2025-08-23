using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using ShapeCraft.Core.Options;
using ShapeCraft.MessageQueue.Services.Contracts;
using ShapeCraft.MessageQueue.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Text.Json;


namespace ShapeCraft.MessageQueue.Services
{
    public class ServiceBusService : IMessageQueueService
    {
        private readonly MessageQueueOptions _options;
        private readonly ServiceBusClient _client;
        private ServiceBusSender _sender;
        private readonly ServiceBusAdministrationClient _adminClient;

        public ServiceBusService(IOptions<MessageQueueOptions> options)
        {
            _options = options.Value;

            // Configure retry options: use exponential backoff with up to 5 retries, starting at 2s delay and maxing out at 30s
            var clientOptions = new ServiceBusClientOptions
            {
                RetryOptions = new ServiceBusRetryOptions
                {
                    Mode = ServiceBusRetryMode.Exponential,
                    MaxRetries = 5,
                    Delay = TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(30)
                }
            };

            //Client | send/receive messages, used at runtime
            //AdminClient | create/delete queue, check if queue exists, used for setup 
            var credential = new DefaultAzureCredential();
            _client = new ServiceBusClient(_options.ServiceBusUrl, credential, clientOptions);
            _adminClient = new ServiceBusAdministrationClient(_options.ServiceBusUrl, credential);
        }

        private async Task EnsureQueueExists(string queueName)
        {
            if (!await _adminClient.QueueExistsAsync(queueName))
            {
                await _adminClient.CreateQueueAsync(queueName);
            }
        }

        public async Task SendMessageAsync(string message)
        {
            try
            {
                await EnsureQueueExists(_options.QueueName);
                if (_sender == null)
                {
                    _sender = _client.CreateSender(_options.QueueName);
                }
                await _sender.SendMessageAsync(new ServiceBusMessage(message));
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occured while sending a message to Service Bus: {ex}");
            }
        }

        public async Task<QueueMessage?> ReceiveQueueMessageAsync()
        {
            await EnsureQueueExists(_options.QueueName);

            var receiver = _client.CreateReceiver(_options.QueueName);

            var message = await receiver.ReceiveMessageAsync(TimeSpan.FromSeconds(5));

            if (message == null)
            {
                return null;
            }

            var queueMessage = JsonSerializer.Deserialize<QueueMessage>(message.Body.ToString());

            await receiver.CompleteMessageAsync(message);

            return queueMessage;
        }

    }
}
