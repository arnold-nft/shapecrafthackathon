using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAI.Services.Contracts
{
    public interface IEmbeddingService
    {
        int Dimensions { get; }
        Task<float[]> EmbedAsync(string text, CancellationToken ct = default);
    }
}
