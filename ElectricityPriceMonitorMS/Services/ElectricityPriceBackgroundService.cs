//ElectricityPriceMonitorMS
//Services/ElectricityPriceBackgroundService.cs
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElectricityPriceMonitorMS.Services
{
    public class ElectricityPriceBackgroundService : IHostedService, IDisposable
    {
        private Timer? _timer;
        private readonly ElectricityPriceService _electricityPriceService;
        private readonly HttpClient _httpClient;

        public ElectricityPriceBackgroundService(ElectricityPriceService electricityPriceService, IHttpClientFactory httpClientFactory)
        {
            _electricityPriceService = electricityPriceService;
            _httpClient = httpClientFactory.CreateClient(); // Initialize the HttpClient
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Run once a day
            // Note: Adjust the TimeSpan to suit when you want the task to first run. Here, it's set to immediately start.
            // TimeSpan.FromDays(1) represents the interval - once every day.
            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromDays(1));

            return Task.CompletedTask;
        }

        private async void DoWork(object? state)
        {
            var prices = await _electricityPriceService.GetLatestPricesAsync();

            // Assuming prices is a List<ElectricityPrice>
            var json = JsonSerializer.Serialize(prices);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "https://localhost:7075/api/prices";
            var response = await _httpClient.PostAsync(url, data);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully sent prices to the second microservice.");
            }
            else
            {
                Console.WriteLine($"Failed to send prices. Status code: {response.StatusCode}");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
            GC.SuppressFinalize(this); //To get rid of (i) message in the console.
        }

    }
}
