using FactoryApp.Application.WeatherService;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases
{
    public interface IWeatherService
    {
        Task<WeatherResponseDTO?> GetCurrentWeatherAsync(WeatherRequest request);
    }
}
