using FactoryApp.Infrastructure.Factories;

namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        IWeatherService CreateBaseService(WeatherServiceCreationRequest request);
    }
}
