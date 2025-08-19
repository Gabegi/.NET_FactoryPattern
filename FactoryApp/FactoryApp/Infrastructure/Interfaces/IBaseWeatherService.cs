
using FactoryApp.Domain.Entities;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Infrastructure.Interfaces
{
    interface IBaseWeatherService
    {
        Task<WeatherResponseDTO> GetForecastAsync(WeatherClientCreationRequest request);
    }
}
