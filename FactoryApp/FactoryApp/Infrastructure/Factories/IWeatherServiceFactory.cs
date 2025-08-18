using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Factories;

public interface IWeatherServiceFactory
{
    IWeatherClient CreateWeatherService(WeatherServiceCreationRequest request);
}
