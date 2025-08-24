using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services.Contracts
{
    public interface ISearchIndexService
    {
        Task EnsureIndexAsync(CancellationToken ct = default);
    }
}
