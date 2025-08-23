using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.MessageQueue.Models
{
    public class QueueMessage
    {
        public string BlobUrl { get; set; }
        public DateTime Timestamp { get; set; }

        public QueueMessage(string blobUrl, DateTime timestamp)
        {
            BlobUrl = blobUrl;
            Timestamp = timestamp;
        }
    }
}
