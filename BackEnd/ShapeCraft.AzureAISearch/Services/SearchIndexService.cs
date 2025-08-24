using Azure;
using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using ShapeCraft.AzureAISearch.Services.Contracts;
using ShapeCraft.Core.Options;

namespace ShapeCraft.AzureAISearch.Services
{
    public sealed record IndexedChunk(
      string id,
      string content,
      string source,
      float[] vector
  );

    public class SearchIndexService : ISearchIndexService
    {
        private readonly AzureSearchOptions _options;
        private readonly SearchIndexClient _indexClient;
        private readonly string _indexName;
        private readonly int _dims;

        public SearchIndexService(AzureSearchOptions azureSearchOptions, string indexName, int dims)
        {
            _options = azureSearchOptions;
            _indexClient = new SearchIndexClient(new Uri(azureSearchOptions.Endpoint), new AzureKeyCredential(azureSearchOptions.ApiKey));
            _indexName = indexName;
            _dims = dims;
        }

        public async Task EnsureIndexAsync(CancellationToken ct = default)
        {
            var indexNames = new List<string>();

            await foreach (var indexName in _indexClient.GetIndexNamesAsync(ct))
            {
                indexNames.Add(indexName);
            }

            if (indexNames.Any(n => n == _indexName)) return;

            var fields = new FieldBuilder().Build(typeof(IndexedChunk));

            var vectorField = new SearchField(nameof(IndexedChunk.vector), SearchFieldDataType.Collection(SearchFieldDataType.Single))
            {
                IsSearchable = true,
                VectorSearchDimensions = _dims,
                VectorSearchProfileName = "vprofile"
            };

            var index = new SearchIndex(_indexName)
            {
                Fields =
            {
                new SimpleField(nameof(IndexedChunk.id), SearchFieldDataType.String) { IsKey = true, IsFilterable = true },
                new SearchField(nameof(IndexedChunk.content), SearchFieldDataType.String) { IsSearchable = true },
                new SearchField(nameof(IndexedChunk.source), SearchFieldDataType.String) { IsFilterable = true },
                vectorField
            },
                VectorSearch = new()
                {
                    Algorithms =
                {
                    new HnswAlgorithmConfiguration("hnsw")
                    {
                        Parameters = new HnswParameters
                        {
                            // Cosine is typical for embeddings
                            Metric = VectorSearchAlgorithmMetric.Cosine,
                            M = 20, EfConstruction = 400, EfSearch = 100
                        }
                    }
                },
                    Profiles = { new VectorSearchProfile("vprofile", "hnsw") }
                }
            };

            await _indexClient.CreateIndexAsync(index, ct);
        }
    }
}