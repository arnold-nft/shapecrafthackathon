using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.OpenseaSync.Services.Contracts
{
    public interface IOpenseaService
    {
        Task<string> GetCollectionAsync(string slug);
    }
}
