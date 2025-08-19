
using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        IWeatherClient CreateBaseClient(WeatherClientCreationRequest request);
    }
}
