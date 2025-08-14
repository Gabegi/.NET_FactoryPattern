using FactoryApp.Infrastructure;
using FactoryApp.Infrastructure.Interfaces;

namespace WeatherApp.Infrastructure
{
    // Interface for the factory
    public interface IWeatherServiceFactory
    {
        IWeatherService Create(string provider);
    }

    // Best Practice Implementation
    public class WeatherServiceFactory : IWeatherServiceFactory
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILoggerFactory _loggerFactory;

        public WeatherServiceFactory(IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IWeatherService Create(string provider)
        {
            if (string.IsNullOrWhiteSpace(provider))
                throw new ArgumentException("Provider cannot be null or empty.", nameof(provider));

            return provider.ToLower() switch
            {
                "openmeteo" => CreateOpenMeteoService(),
                "weatherapi" => CreateWeatherApiService(),
                _ => throw new NotSupportedException($"Weather provider '{provider}' is not supported.")
            };
        }

        private OpenMeteoService CreateOpenMeteoService()
        {
            var httpClient = _httpClientFactory.CreateClient("OpenMeteo");
            var logger = _loggerFactory.CreateLogger<OpenMeteoService>();
            return new OpenMeteoService(httpClient, logger);
        }

        private WeatherApiService CreateWeatherApiService()
        {
            var httpClient = _httpClientFactory.CreateClient("WeatherAPI");
            var logger = _loggerFactory.CreateLogger<WeatherApiService>();
            return new WeatherApiService(httpClient, logger);
        }
    }
}
