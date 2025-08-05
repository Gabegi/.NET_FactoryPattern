using KeyedDependencyInjectionApp.Domain.Entities;
using KeyedDependencyInjectionApp.Infrastructure.Interfaces;
using KeyedDependencyInjectionApp.Presentation.Controllers.DTOs;

namespace KeyedDependencyInjectionApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IServiceProvider _serviceProvider;

    public GetCurrentWeatherUseCase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<WeatherResponseDTO?> ExecuteAsync(double latitude, double longitude, string serviceKey = "openmeteo")
    {
        // Application logic can be added here (validation, business rules, etc.)
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees");

        // Keyed dependency injection - get the service by key
        var weatherService = _serviceProvider.GetRequiredKeyedService<IWeatherService>(serviceKey);
        
        var weather = await weatherService.GetCurrentWeatherAsync(latitude, longitude);
        if (weather == null) return null;

        return new WeatherResponseDTO
        {
            Latitude = weather.Latitude,
            Longitude = weather.Longitude,
            Generationtime_ms = weather.Generationtime_ms,
            Utc_offset_seconds = weather.Utc_offset_seconds,
            Timezone = weather.Timezone,
            Timezone_abbreviation = weather.Timezone_abbreviation,
            Elevation = weather.Elevation,
            Current_weather_units = new CurrentWeatherUnitsDto
            {
                Time = weather.Current_weather_units.Time,
                Interval = weather.Current_weather_units.Interval,
                Temperature = weather.Current_weather_units.Temperature,
                Windspeed = weather.Current_weather_units.Windspeed,
                Winddirection = weather.Current_weather_units.Winddirection,
                Is_day = weather.Current_weather_units.Is_day,
                Weathercode = weather.Current_weather_units.Weathercode
            },
            Current_weather = new CurrentWeatherDto
            {
                Time = weather.Current_weather.Time,
                Interval = weather.Current_weather.Interval,
                Temperature = weather.Current_weather.Temperature,
                Windspeed = weather.Current_weather.Windspeed,
                Winddirection = weather.Current_weather.Winddirection,
                Is_day = weather.Current_weather.Is_day,
                Weathercode = weather.Current_weather.Weathercode
            }
        };
    }
} 