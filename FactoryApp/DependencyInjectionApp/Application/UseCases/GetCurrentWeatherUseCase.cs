using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherService _weatherService;

    public GetCurrentWeatherUseCase(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<Weather?> ExecuteAsync(double latitude, double longitude)
    {
        // Application logic can be added here (validation, business rules, etc.)
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees");

        var weather = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);
        if (weather == null) return null;

        return new Weather
        {
            Latitude = weather.Latitude,
            Longitude = weather.Longitude,
            Generationtime_ms = weather.Generationtime_ms,
            Utc_offset_seconds = weather.Utc_offset_seconds,

        };
    }
} 