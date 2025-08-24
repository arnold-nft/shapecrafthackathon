using ShapeCraft.AzureAI.Services.Contracts;
using ShapeCraft.AzureAISearch.Models;
using ShapeCraft.AzureAISearch.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services
{
    public class IngestionService : IIngestionService
    {
        private readonly IEmbeddingService _embeddings;
        private readonly IVectorSearchService _vectorStore;

        public IngestionService(IEmbeddingService embeddingsService, IVectorSearchService vectorSearchService)
        {
            _embeddings = embeddingsService;
            _vectorStore = vectorSearchService;
        }

        public async Task IngestAsync(string sourceId, string rawText, CancellationToken ct = default)
        {
            foreach (var batch in Chunk(rawText, maxChars: 1200, overlap: 150).Chunk(32)) // tiny batches
            {
                var docs = new List<IndexedChunk>();
                foreach (var (text, i) in batch.Select((t, i) => (t, i)))
                {
                    var vec = await _embeddings.EmbedAsync(text, ct);

                    string sanitizedSourceId = sourceId
                        .Replace("https://", "")
                        .Replace("http://", "")
                        .Replace("/", "-")
                        .Replace(".", "-");

                    docs.Add(new IndexedChunk(
                        id: $"{sanitizedSourceId}-{Guid.NewGuid():N}",
                        content: text,
                        source: sourceId,
                        vector: vec
                    ));
                }
                await _vectorStore.UpsertAsync(docs, ct);
            }
        }

        private static IEnumerable<string> Chunk(string text, int maxChars, int overlap)
        {
            if (string.IsNullOrWhiteSpace(text)) yield break;
            int start = 0;
            while (start < text.Length)
            {
                int len = Math.Min(maxChars, text.Length - start);
                yield return text.Substring(start, len);
                start += (maxChars - overlap);
            }
        }
    }
}