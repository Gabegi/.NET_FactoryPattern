using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Factories;

public interface IWeatherClientFactory
{
    IWeatherClient CreateWeatherService(WeatherServiceCreationRequest request);
}
