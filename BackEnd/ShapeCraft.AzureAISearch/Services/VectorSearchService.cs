using Azure;
using Azure.Core;
using Azure.Search.Documents;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Models;
using ShapeCraft.AzureAISearch.Models;
using ShapeCraft.AzureAISearch.Services.Contracts;
using ShapeCraft.Core.Options;

namespace ShapeCraft.AzureAISearch.Services
{
    public sealed class VectorSearchService : IVectorSearchService
    {
        private readonly SearchClient _searchClient;
        private readonly AzureSearchOptions _options;

        public VectorSearchService(AzureSearchOptions azureSearchOptions)
        {
            _options = azureSearchOptions;
            _searchClient = new SearchClient(new Uri(azureSearchOptions.Endpoint), azureSearchOptions.IndexName, new AzureKeyCredential(azureSearchOptions.ApiKey));
        }

        public async Task UpsertAsync(IEnumerable<IndexedChunk> docs, CancellationToken ct = default)
        {
            var options = new IndexDocumentsOptions
            {
                // You can add any other options here if necessary
            };

            await _searchClient.MergeOrUploadDocumentsAsync(docs, options, ct);
        }

        public async Task<IReadOnlyList<IndexedChunk>> SearchAsync(float[] queryVector, int k = 5, CancellationToken ct = default)
        {
            var options = new SearchOptions
            {
                Size = k,
                VectorSearch = new VectorSearchOptions
                {
                    Queries =
                {
                    new VectorizedQuery(queryVector)
                    {
                        Fields = { nameof(IndexedChunk.vector) },
                        KNearestNeighborsCount = k
                    }
                }
                },
                Select = { nameof(IndexedChunk.id), nameof(IndexedChunk.content), nameof(IndexedChunk.source) }
            };

            var results = await _searchClient.SearchAsync<SearchDocument>(null, options, ct);
            var list = new List<IndexedChunk>();
            await foreach (var r in results.Value.GetResultsAsync())
            {
                var doc = r.Document;
                list.Add(new IndexedChunk(
                    id: (string)doc[nameof(IndexedChunk.id)],
                    content: (string)doc[nameof(IndexedChunk.content)],
                    source: (string)doc[nameof(IndexedChunk.source)],
                    vector: Array.Empty<float>()
                ));
            }
            return list;
        }
    }

}
