using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Infrastructure;

public class MockWeatherService : IWeatherService
{
    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        await Task.Delay(100); // Simulate API delay

        return new Weather
        {
            Latitude = latitude,
            Longitude = longitude,
            Generationtime_ms = 0.234,
            Utc_offset_seconds = 3600,
            Timezone = "Europe/Berlin",
            Timezone_abbreviation = "CEST",
            Elevation = 100.0,
            Current_weather_units = new CurrentWeatherUnits
            {
                Time = "iso8601",
                Interval = "s",
                Temperature = "°C",
                Windspeed = "km/h",
                Winddirection = "°",
                Is_day = "bool",
                Weathercode = "code"
            },
            Current_weather = new CurrentWeather
            {
                Time = DateTime.UtcNow.ToString("o"),
                Interval = 60,
                Temperature = 21.4,
                Windspeed = 5.5,
                Winddirection = 180,
                Is_day = 1,
                Weathercode = 2
            }
        };
    }
}
