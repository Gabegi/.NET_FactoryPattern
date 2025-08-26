using FactoryApp.Application;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClientFactory
{
    IWeatherClient CreateWeatherService(WeatherClientCreationRequest request);
}
