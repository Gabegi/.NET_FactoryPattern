using KeyedDependencyInjectionApp.Domain.Entities;

namespace KeyedDependencyInjectionApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude);
} 