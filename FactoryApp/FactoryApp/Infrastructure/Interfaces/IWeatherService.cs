using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude);
    bool SupportsFeature(string feature);
} 