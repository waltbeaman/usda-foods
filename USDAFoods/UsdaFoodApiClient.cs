using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace USDAFoods
{
    internal class UsdaFoodApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public UsdaFoodApiClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<string> SearchFood(string query)
        {
            string apiKey = _config["UsdaApiKey"];
            string url = $"https://api.nal.usda.gov/fdc/v1/foods/search?api_key={apiKey}&query={query}";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
