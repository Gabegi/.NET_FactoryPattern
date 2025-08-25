using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClientFactory
{
    IWeatherClient CreateWeatherService(WeatherClientCreationRequest request);
}
