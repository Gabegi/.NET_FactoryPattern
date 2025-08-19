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

        public BaseClientConfig(
        IServiceProvider serviceProvider,
        ILogger<WeatherClientFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public IWeatherClient CreateClient(WeatherClientCreationRequest request)
        {
            if (!Enum.TryParse<WeatherServiceType>(request.ServiceName, out var serviceType))
            {
                throw new ArgumentException($"Invalid service name: {request.ServiceName}");
            }

            var config = GetConfig(serviceType);

            _logger.LogInformation(
                "Creating WeatherClient for {ServiceName} ({Region}, {Env})",
                serviceName, config.Region, config.Environment);

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return new WeatherClient(httpClient, config);
        }

        private static WeatherServiceConfigAttribute GetConfig(WeatherServiceType serviceType)
        {
            var memberInfo = typeof(WeatherServiceType)
                .GetMember(serviceType.ToString())
                .FirstOrDefault();

            var attribute = memberInfo?
                .GetCustomAttribute<WeatherServiceConfigAttribute>();

            if (attribute == null)
            {
                throw new InvalidOperationException($"No config found for {serviceType}");
            }

            return attribute;
        }
    }
}
}
