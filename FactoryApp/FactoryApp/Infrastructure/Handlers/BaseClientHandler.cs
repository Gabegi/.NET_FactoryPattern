using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using System.Net.Http;

namespace FactoryApp.Infrastructure.Handlers
{
    public class BaseClientHandler : IBaseClient
    {
        private readonly ILogger<WeatherClientFactory> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientHandler(
        ILogger<WeatherClientFactory> logger,
        IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
        {
            _logger.LogInformation($"Creating WeatherClient for {request.ServiceName} ({request.Region}, {request.Environment})");

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return httpClient;
        }

    }
}

