using Azure.Identity;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ShapeCraft.Core.Options;
using ShapeCraft.Storage.Services.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AzureIndexer.Storage.Services
{
    public class BlobService : IBlobService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly ILogger<BlobService> _logger;
        private readonly BlobStorageOptions _storageOptions;
        private readonly DefaultAzureCredential _azureCredential;

        public BlobService(BlobStorageOptions blobStorageOptions, IOptions<JsonSerializerOptions> jsonOptions, ILogger<BlobService> logger)
        {
            _azureCredential = new DefaultAzureCredential();

            _containerClient = new BlobContainerClient(
                new Uri($"{blobStorageOptions.BlobStorageUrl}/{blobStorageOptions.BlobContainerName}"),
                _azureCredential
            );

            _logger = logger;
            _storageOptions = blobStorageOptions;

            // Create the container if it does not exist
            _containerClient.CreateIfNotExists();
        }

        public async Task<bool> EnsureExternalConnection(ILogger logger)
        {
            try
            {
                var blob = new BlobContainerClient(
                    new Uri($"{_storageOptions.BlobStorageUrl}/{_storageOptions.BlobContainerName}"),
                    _azureCredential
                );

                logger.LogInformation("TableClient Created");
                await blob.CreateIfNotExistsAsync();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to ensure Table Storage external connection for connector: {_storageOptions.BlobContainerName} ");
                return false;
            }
        }


        public async Task<Uri> UploadItemAsync(string json, string blobName)
        {
            try
            {
                var containerName = "shapecraft-files";
                var blobServiceClient = new BlobServiceClient(new Uri(_storageOptions.BlobStorageUrl), new DefaultAzureCredential());

                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);
                await containerClient.CreateIfNotExistsAsync();

                var blobClient = containerClient.GetBlobClient(blobName);

                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                var options = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = "application/json",
                    }
                };

                await blobClient.UploadAsync(stream, options);
                _logger.LogInformation("Uploaded item to blob successfully:" + blobName);

                return blobClient.Uri;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to upload item:" + blobName);
                throw;
            }
        }


        public async Task<string> DownloadBlobContentAsync(string blobUrl)
        {
            try
            {
                var blobClient = new BlobClient(new Uri(blobUrl), _azureCredential);
                var blobDownload = await blobClient.DownloadContentAsync();
                var content = blobDownload.Value.Content.ToString();

                return content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to download blob content from URL: {blobUrl}");
                throw;
            }
        }



    }
}
