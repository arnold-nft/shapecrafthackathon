using ShapeCraft.Core.Options;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ShapeCraft.OpenseaSync.Services.Contracts;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ShapeCraft.OpenseaSync
{
    public class OpenseaService : IOpenseaService
    {
        private readonly HttpClient _httpClient;
        private readonly OpenSeaOptions _options;
        private readonly ILogger<OpenseaService> _logger;

        public OpenseaService(HttpClient httpClient, OpenSeaOptions options, ILogger<OpenseaService> logger)
        {
            _httpClient = httpClient;
            _options = options;
            _logger = logger;

            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _httpClient.DefaultRequestHeaders.Add("x-api-key", _options.ApiKey);
        }

        public async Task<string> GetCollectionAsync(string slug)
        {
            var url = $"/api/v2/collections/{slug}";
            _logger.LogInformation("Fetching collection metadata from OpenSea: {Slug}", slug);

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Successfully fetched collection data for slug: {Slug}", slug);
                var content = await response.Content.ReadAsStringAsync();

                return content;
            }
            else
            {
                var errorMessage = $"Error fetching collection metadata for {slug}. Status code: {response.StatusCode}";
                _logger.LogError(errorMessage);
                throw new HttpRequestException(errorMessage);
            }
        }
    }
}
