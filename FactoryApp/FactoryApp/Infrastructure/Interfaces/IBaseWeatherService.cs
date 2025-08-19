
using FactoryApp.Domain.Entities;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config);
    }
}
