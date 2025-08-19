using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;
using System.Net.Http;
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

        public async Task<WeatherResponseDTO> GetForecastAsync(WeatherClientCreationRequest request)
        {
            var httpClient = CreateClient(request);

            var response = await httpClient.GetAsync(
                $"?lat={_config.Latitude}&lon={_config.Longitude}");

            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        private HttpClient CreateClient(WeatherClientCreationRequest request)
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

            return httpClient;
        }

        private static WeatherServiceConfigAttribute GetConfig(WeatherServiceType serviceType) 
        { 
            var memberInfo = typeof(WeatherServiceType).GetMember(serviceType.ToString()).FirstOrDefault(); 
            var attribute = memberInfo?.GetCustomAttribute<WeatherServiceConfigAttribute>(); 
            if (attribute == null) 
            { throw new InvalidOperationException($"No config found for {serviceType}"); } 
            return attribute; 
        }


    }
}

