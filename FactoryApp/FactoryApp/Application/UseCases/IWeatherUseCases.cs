using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases
{
    public interface IWeatherUseCases
    {
        Task<WeatherResponseDTO?> GetWeatherAsync();
    }
}
