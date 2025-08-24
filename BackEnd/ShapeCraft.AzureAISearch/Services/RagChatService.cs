using Azure.AI.OpenAI;
using OpenAI.Chat;
using ShapeCraft.AzureAI.Services.Contracts;
using ShapeCraft.AzureAISearch.Services.Contracts;
using ShapeCraft.Core.Options;
using System;
using System.ClientModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services
{
    public sealed class RagChatService : IRagChatService
    {
        private readonly IEmbeddingService _embeddings;
        private readonly IVectorSearchService _search;
        private readonly ChatClient _chat;
        private readonly AzureAIOptions _azureAIOptions;

        public RagChatService(IEmbeddingService embeddingService, IVectorSearchService search, AzureAIOptions azureAIOptions)
        {
            _embeddings = embeddingService;
            _search = search;
            _azureAIOptions = azureAIOptions;
            var azureOpenAiClient = new AzureOpenAIClient(new Uri(_azureAIOptions.AzureAIUrl), new ApiKeyCredential(_azureAIOptions.ApiKey));
            _chat = azureOpenAiClient.GetChatClient(azureAIOptions.ChatDeployment);
        }

        public async Task<string> AskAsync(string question, int k = 5, CancellationToken ct = default)
        {
            var qVec = await _embeddings.EmbedAsync(question, ct);
            var passages = await _search.SearchAsync(qVec, k, ct);

            var sb = new StringBuilder();
            foreach (var p in passages)
                sb.AppendLine($"- {p.content}");

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You answer strictly from the provided context. If unsure, say you don't know."),
                new UserChatMessage($"Question: {question}\n\nContext:\n{sb}")
            };

            var options = new ChatCompletionOptions
            {
                // Set any additional options here if needed
            };

            var completion = await _chat.CompleteChatAsync(messages, options, ct);

            var chatCompletion = completion.Value;

            var text = chatCompletion.Content[0].Text;

            return text;
        }
    }
}
