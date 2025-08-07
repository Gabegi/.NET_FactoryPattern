using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure;

public class MockWeatherService : IWeatherService
{
    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        // Simulate API delay
        await Task.Delay(100);

        return new Weather
        {
            Latitude = latitude,
            Longitude = longitude,
            Generationtime_ms = 0.123456789,
            Utc_offset_seconds = 0,
            Timezone = "UTC",
            Timezone_abbreviation = "UTC",
            Elevation = 0,
            Current_weather_units = new CurrentWeatherUnits
            {
                Time = "iso8601",
                Interval = "seconds",
                Temperature = "celsius",
                Windspeed = "kmh",
                Winddirection = "degrees",
                Is_day = "",
                Weathercode = "wmo code"
            },
            Current_weather = new CurrentWeather
            {
                Time = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm"),
                Interval = 900,
                Temperature = 20.5,
                Windspeed = 15.2,
                Winddirection = 180,
                Is_day = 1,
                Weathercode = 1
            }
        };
    }
} 