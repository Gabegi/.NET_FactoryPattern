using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace FactoryApp.Infrastructure.Factories;

public class WeatherServiceFactory : IWeatherServiceFactory
{
    private readonly IServiceProvider _serviceProvider;

    public WeatherServiceFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IWeatherService CreateWeatherService(string serviceType)
    {
        return serviceType.ToLowerInvariant() switch
        {
            "mock" => _serviceProvider.GetRequiredService<MockWeatherService>(),
            "openmeteo" => _serviceProvider.GetRequiredService<OpenMeteoService>(),
            _ => throw new ArgumentException($"Unknown weather service type: {serviceType}")
        };
    }
} 