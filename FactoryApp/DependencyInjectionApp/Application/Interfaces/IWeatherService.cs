using DependencyInjectionApp.Controllers.DTOs;

namespace DependencyInjectionApp.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherInfo?> GetCurrentWeatherAsync(double latitude, double longitude);
} 