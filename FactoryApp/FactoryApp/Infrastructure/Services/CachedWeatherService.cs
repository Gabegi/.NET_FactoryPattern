using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services;

public class CachedWeatherService : IWeatherService
{
    private readonly IWeatherService _weatherService;
    private readonly ICacheService _cacheService;

    public CachedWeatherService(IWeatherService weatherService, ICacheService cacheService)
    {
        _weatherService = weatherService;
        _cacheService = cacheService;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        var cacheKey = $"weather_{latitude}_{longitude}";
        
        // Try to get from cache first
        var cachedWeather = await _cacheService.GetAsync<Weather>(cacheKey);
        if (cachedWeather != null)
        {
            return cachedWeather;
        }

        // If not in cache, get from service and cache it
        var weather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
        if (weather != null)
        {
            await _cacheService.SetAsync(cacheKey, weather, TimeSpan.FromMinutes(30));
        }

        return weather;
    }

    public bool SupportsFeature(string feature)
    {
        return _weatherService.SupportsFeature(feature);
    }
}
