using ShapeCraft.MessageQueue.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.MessageQueue.Services.Contracts
{
    public interface IMessageQueueService
    {
        Task SendMessageAsync(string message);
        Task<QueueMessage?> ReceiveQueueMessageAsync();
    }
}
