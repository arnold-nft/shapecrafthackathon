using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.Core.Options
{
    public class AzureAIOptions
    {
        public string AzureAIUrl { get; set; }
        public string ApiKey { get; set; }
        public string EmbeddingDeployment { get; set; }
        public string ChatDeployment { get; set; }
    }
}
