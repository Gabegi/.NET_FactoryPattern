using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClient
{
    Task<Weather?> GetCurrentWeatherAsync(string serviceName);
    bool SupportsFeature(string feature);
} 