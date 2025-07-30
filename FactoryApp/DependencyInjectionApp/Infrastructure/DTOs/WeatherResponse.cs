namespace DependencyInjectionApp.Infrastructure.DTOs;

public class WeatherResponse
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Generationtime_ms { get; set; }
    public int Utc_offset_seconds { get; set; }
    public string Timezone { get; set; } = string.Empty;
    public string Timezone_abbreviation { get; set; } = string.Empty;
    public double Elevation { get; set; }
    public CurrentWeatherUnits Current_weather_units { get; set; } = new();
    public CurrentWeather Current_weather { get; set; } = new();
}

public class CurrentWeatherUnits
{
    public string Time { get; set; } = string.Empty;
    public string Interval { get; set; } = string.Empty;
    public string Temperature { get; set; } = string.Empty;
    public string Windspeed { get; set; } = string.Empty;
    public string Winddirection { get; set; } = string.Empty;
    public string Is_day { get; set; } = string.Empty;
    public string Weathercode { get; set; } = string.Empty;
}

public class CurrentWeather
{
    public string Time { get; set; } = string.Empty;
    public int Interval { get; set; }
    public double Temperature { get; set; }
    public double Windspeed { get; set; }
    public int Winddirection { get; set; }
    public int Is_day { get; set; }
    public int Weathercode { get; set; }
} 