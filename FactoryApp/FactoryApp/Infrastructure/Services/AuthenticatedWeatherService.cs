using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services;

public class AuthenticatedWeatherService : IWeatherClient
{
    private readonly IWeatherClient _weatherService;

    public AuthenticatedWeatherService(IWeatherClient weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        // In a real implementation, you would add authentication logic here
        // For now, we'll just pass through to the underlying service
        return await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
    }

    public bool SupportsFeature(string feature)
    {
        return _weatherService.SupportsFeature(feature);
    }
}
