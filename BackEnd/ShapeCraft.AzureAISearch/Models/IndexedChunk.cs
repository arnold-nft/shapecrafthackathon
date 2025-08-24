using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Models
{
    public sealed record IndexedChunk
    (
        string id,
        string content,
        string source,
        float[] vector
    );
}
