using DependencyInjectionApp.Infrastructure.DTOs;

namespace DependencyInjectionApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<WeatherResponse?> GetCurrentWeatherAsync(double latitude, double longitude);
} 