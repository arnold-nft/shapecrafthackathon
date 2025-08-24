using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.AzureAISearch.Services.Contracts
{
    public interface IIngestionService
    {
        Task IngestAsync(string sourceId, string rawText, CancellationToken ct = default);
    }
}
