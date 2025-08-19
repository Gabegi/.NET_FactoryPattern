using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services;

public class CachedWeatherService : ICacheService
{
    private readonly IWeatherClient _weatherService;

    public CachedWeatherService(IWeatherClient weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
    }
 
}
