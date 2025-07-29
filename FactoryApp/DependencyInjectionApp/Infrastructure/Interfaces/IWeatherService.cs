using DependencyInjectionApp.Infrastructure.DTOs;

namespace DependencyInjectionApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<OpenMeteoResponse?> GetCurrentWeatherAsync(double latitude, double longitude);
} 