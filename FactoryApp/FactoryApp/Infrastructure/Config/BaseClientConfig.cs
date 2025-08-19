using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using System.Reflection;

namespace FactoryApp.Infrastructure.Services
{
    public class BaseClientConfig : IBaseWeatherService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherClientFactory> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientConfig(
        IServiceProvider serviceProvider,
        ILogger<WeatherClientFactory> logger,
        IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public IWeatherClient CreateClient(WeatherClientCreationRequest request)
        {
            if (!Enum.TryParse<WeatherServiceType>(request.ServiceName, out var serviceType))
            {
                throw new ArgumentException($"Invalid service name: {request.ServiceName}");
            }

            var config = GetConfig(serviceType);

            _logger.LogInformation(
               $"Creating WeatherClient for {request.ServiceName} ({config.Region}, {config.Environment})");

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return new OpenMeteoClient(httpClient, config);
        }
    }
}

