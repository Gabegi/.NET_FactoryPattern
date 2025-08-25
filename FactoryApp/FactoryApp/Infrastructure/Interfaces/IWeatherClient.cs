using FactoryApp.Domain.Entities;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClient
{
    Task<Weather?> GetCurrentWeatherAsync(WeatherRequestDTO request);
} 