using WeatherApp.Application.Interfaces;

namespace FactoryApp.Infrastructure.Factories
{
    public interface IWeatherServiceFactory
    {
        IWeatherService Create();
    }
}
