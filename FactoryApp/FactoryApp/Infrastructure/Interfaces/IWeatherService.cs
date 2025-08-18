using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync(string serviceName);
    bool SupportsFeature(string feature);
} 