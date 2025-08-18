using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services
{
    public class BaseWeatherService : IBaseWeatherService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<WeatherServiceFactory> _logger;

        public BaseWeatherService(
        IServiceProvider serviceProvider,
        ILogger<WeatherServiceFactory> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        // STEP 1: Complex conditional base service creation
        //IWeatherService IBaseWeatherService.CreateBaseService(string serviceName)
        //{
        //    return CreateBaseService(serviceName);
        //}
        internal IWeatherClient CreateBaseService(string serviceName)
        {
            // Mock service for development/testing
            if (IsNonProductionEnvironment(serviceName))
            {
                _logger.LogDebug("Using mock weather service for non-production environment");
                return _serviceProvider.GetRequiredService<MockWeatherService>();
            }

            // Production services - complex regional logic
            return request.Region.ToLowerInvariant() switch
            {
                "europe" or "eu" => CreateEuropeanService(request),
                "northamerica" or "na" => CreateNorthAmericanService(request),
                "asia" => CreateAsianService(request),
                "global" => CreateGlobalService(request),
                _ => CreateDefaultService(request)
            };
        }

        internal bool IsNonProductionEnvironment(string environment)
        {
            return environment.ToLowerInvariant() switch
            {
                "development" or "dev" or "test" or "staging" => true,
                _ => false
            };
        }

        internal IWeatherClient CreateTokyoService(WeatherServiceCreationRequest request)
        {
            _logger.LogDebug("Creating European weather service");
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }

        internal IWeatherClient CreateNewYorkService(WeatherServiceCreationRequest request)
        {
            _logger.LogDebug("Creating North American weather service");
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }

        internal IWeatherClient CreateAsianService(WeatherServiceCreationRequest request)
        {
            _logger.LogDebug("Creating Asian weather service");
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }

        internal IWeatherClient CreateGlobalService(WeatherServiceCreationRequest request)
        {
            _logger.LogDebug("Creating global weather service");
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }

        internal IWeatherClient CreateDefaultService(WeatherServiceCreationRequest request)
        {
            _logger.LogDebug("Creating default weather service");
            return _serviceProvider.GetRequiredService<OpenMeteoClient>();
        }


    }
}
