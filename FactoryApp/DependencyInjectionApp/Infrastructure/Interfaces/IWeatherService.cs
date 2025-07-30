using DependencyInjectionApp.Domain.Entities;

namespace DependencyInjectionApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude);
} 