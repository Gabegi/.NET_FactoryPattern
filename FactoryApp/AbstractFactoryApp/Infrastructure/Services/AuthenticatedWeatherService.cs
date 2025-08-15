using AbstractFactoryApp.Domain.Entities;
using AbstractFactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AbstractFactoryApp.Infrastructure.Services;

public class AuthenticatedWeatherService : IWeatherService
{
    private readonly IWeatherService _weatherService;
    private readonly IConfiguration _configuration;

    public AuthenticatedWeatherService(IWeatherService weatherService, IConfiguration configuration)
    {
        _weatherService = weatherService;
        _configuration = configuration;
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
