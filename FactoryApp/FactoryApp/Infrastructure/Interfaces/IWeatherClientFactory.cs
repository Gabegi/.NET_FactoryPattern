using FactoryApp.Application.WeatherService;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClientFactory
{
    IWeatherClient CreateWeatherService(WeatherRequest request);
}
