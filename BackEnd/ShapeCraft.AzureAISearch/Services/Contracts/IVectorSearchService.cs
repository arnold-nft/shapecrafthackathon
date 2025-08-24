using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services.Contracts
{
    public interface IVectorSearchService
    {
        Task UpsertAsync(IEnumerable<IndexedChunk> docs, CancellationToken ct = default);
        Task<IReadOnlyList<IndexedChunk>> SearchAsync(float[] queryVector, int k = 5, CancellationToken ct = default);
    }
}
