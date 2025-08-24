using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services.Contracts
{
    public interface IRagChatService
    {
        Task<string> AskAsync(string question, int k = 5, CancellationToken ct = default);
    }
}
