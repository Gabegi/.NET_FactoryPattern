using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;
using System.Reflection;
using System.Text.Json;

namespace FactoryApp.Infrastructure.Services
{
    public class BaseClientConfig : IBaseWeatherService
    {
        private readonly ILogger<WeatherClientFactory> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientConfig(
        ILogger<WeatherClientFactory> logger,
        IConfiguration configuration,
        IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _configuration = configuration;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<WeatherResponseDTO> GetForecastAsync(WeatherClientCreationRequest request)
        {
            var httpClient = CreateClient(request);
            var url = _configuration["WeatherApi:Url"] ?? throw new InvalidOperationException("WeatherApi:Url not found in appsettings");

            var response = await httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStreamAsync();

            var result = JsonSerializer.Deserialize<WeatherResponseDTO?>(content, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return result ?? throw new InvalidOperationException("Failed to deserialize weather response - received null");
        } 
        
        private HttpClient CreateClient(WeatherClientCreationRequest request)
        {
            _logger.LogInformation($"Creating WeatherClient for {request.ServiceName} ({request.Region}, {request.Environment})");
            
            var config = GetConfig(request);

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return httpClient;
        }

        private static WeatherServiceConfigAttribute GetConfig(WeatherClientCreationRequest request) 
        {
            if (!Enum.TryParse<WeatherServiceType>(request.ServiceName, out var serviceType))
            {
                throw new ArgumentException($"Invalid service name: {request.ServiceName}");
            }

            var memberInfo = typeof(WeatherServiceType).GetMember(serviceType.ToString()).FirstOrDefault(); 
            var attribute = memberInfo?.GetCustomAttribute<WeatherServiceConfigAttribute>(); 
            if (attribute == null) 
            { throw new InvalidOperationException($"No config found for {serviceType}"); } 
            return attribute; 
        }


    }
}

