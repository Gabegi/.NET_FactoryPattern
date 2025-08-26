using FactoryApp.Application.WeatherService;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClient
{
    Task<WeatherResponse?> GetCurrentWeatherAsync(WeatherRequestDTO request);
} 