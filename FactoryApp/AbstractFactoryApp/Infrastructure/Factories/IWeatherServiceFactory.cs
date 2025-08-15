using AbstractFactoryApp.Infrastructure.Interfaces;

namespace AbstractFactoryApp.Infrastructure.Factories;

public interface IWeatherServiceFactory
{
    IWeatherService CreateWeatherService(WeatherServiceCreationRequest request);
}
