using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapeCraft.Storage.Services.Contracts
{
    public interface IBlobService
    {
        Task<Uri> UploadItemAsync(string json, string blobName);
        Task<string> DownloadBlobContentAsync(string blobUrl);
    }
}
