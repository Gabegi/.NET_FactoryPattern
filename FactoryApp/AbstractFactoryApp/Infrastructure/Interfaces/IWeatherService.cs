using AbstractFactoryApp.Domain.Entities;

namespace AbstractFactoryApp.Infrastructure.Interfaces;

public interface IWeatherService
{
    Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude);
    bool SupportsFeature(string feature);
}
