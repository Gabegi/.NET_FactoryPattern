using DependencyInjectionApp.Domain.Entities;

namespace DependencyInjectionApp.Domain.Interfaces;

public interface IWeatherRepository
{
    Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude);
} 