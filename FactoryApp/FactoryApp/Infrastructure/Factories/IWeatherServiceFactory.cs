using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Factories;

public interface IWeatherServiceFactory
{
    IWeatherService CreateWeatherService(string serviceType);
}
