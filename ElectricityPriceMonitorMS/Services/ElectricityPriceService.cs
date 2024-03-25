//ElectricityPriceMonitorMS
//ElectricityPriceService.cs
using ElectricityPriceMonitorMS.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ElectricityPriceMonitorMS.Services
{
    public class ElectricityPriceService
    {
        private readonly HttpClient _httpClient;

        public ElectricityPriceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ElectricityPrice>> GetLatestPricesAsync()
        {
            var responseStream = await _httpClient.GetStreamAsync("https://api.porssisahko.net/v1/latest-prices.json");
            var pricesResponse = await JsonSerializer.DeserializeAsync<ElectricityPricesResponse>(responseStream);
            return pricesResponse?.Prices ?? new List<ElectricityPrice>();
        }
    }
    //Wrapper class
    public class ElectricityPricesResponse
    {
        [JsonPropertyName("prices")]
        public List<ElectricityPrice> Prices { get; set; }
    }
}


