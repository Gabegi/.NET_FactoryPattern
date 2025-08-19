using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services
{
    public class BaseClientProvider : IBaseClient
    {
        private readonly ILogger<WeatherClientFactory> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientProvider(
        ILogger<WeatherClientFactory> logger,
        IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
        {
            _logger.LogInformation($"Creating WeatherClient for {request.ServiceName} ({request.Region}, {request.Environment})");

            var httpClient = request.EnableCaching
                ? _httpClientFactory.CreateClient("WeatherApiCached")  // With caching handler
                : _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            switch (request.ServiceName.ToLower())
            {
                case "openweathermap":
                    ConfigureOpenWeatherMapClient(client, request);
                    break;
                case "weatherapi":
                    ConfigureWeatherApiClient(client, request);
                    break;
                case "accuweather":
                    ConfigureAccuWeatherClient(client, request);
                    break;
                default:
                    ConfigureDefaultClient(client, request);
                    break;
            }

            return httpClient;
        }
    }
}

