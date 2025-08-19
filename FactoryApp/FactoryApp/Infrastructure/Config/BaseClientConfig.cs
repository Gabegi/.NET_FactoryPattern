using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;
using System.Text.Json;

namespace FactoryApp.Infrastructure.Services
{
    public class BaseClientConfig : IBaseWeatherService
    {
        private readonly ILogger<WeatherClientFactory> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientConfig(
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

