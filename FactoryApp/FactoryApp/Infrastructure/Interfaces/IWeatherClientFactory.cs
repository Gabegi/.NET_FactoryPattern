namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClientFactory
{
    IWeatherClient CreateWeatherService(WeatherServiceCreationRequest request);
}
