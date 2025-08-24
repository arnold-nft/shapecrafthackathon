using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.Core.Options
{
    public class MessageQueueOptions
    {
        public string ServiceBusUrl { get; set; }
        public string QueueName { get; set; }
    }
}
