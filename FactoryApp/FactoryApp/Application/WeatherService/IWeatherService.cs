using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases
{
    public interface IWeatherService
    {
        Task<WeatherResponseDTO?> GetWeatherAsync(WeatherRequestDTO request);
    }
}
