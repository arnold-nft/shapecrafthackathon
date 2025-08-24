using Azure.AI.OpenAI;
using OpenAI.Embeddings;
using ShapeCraft.Core.Options;
using System.ClientModel;
using ShapeCraft.AzureAI.Services.Contracts;

public sealed class EmbeddingService : IEmbeddingService
{
    private readonly EmbeddingClient _embeddingClient;
    private readonly AzureOpenAIClient _azureOpenAIClient;
    private readonly AzureAIOptions _azureAIOptions;
    public int Dimensions { get; }

    public EmbeddingService(AzureAIOptions azureAIOptions)
    {
        _azureAIOptions = azureAIOptions;
        _azureOpenAIClient = new AzureOpenAIClient(new Uri(_azureAIOptions.AzureAIUrl), new ApiKeyCredential(_azureAIOptions.ApiKey));
        _embeddingClient = _azureOpenAIClient.GetEmbeddingClient(_azureAIOptions.EmbeddingDeployment);
        Dimensions = 3072;
    }

    public async Task<float[]> EmbedAsync(string text, CancellationToken ct = default)
    {
        var result = await _embeddingClient.GenerateEmbeddingAsync(text, cancellationToken: ct);

        var embeddings = result.Value.ToFloats();

        return embeddings.ToArray();
    }
}
