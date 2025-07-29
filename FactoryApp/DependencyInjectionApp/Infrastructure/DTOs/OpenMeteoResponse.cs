namespace DependencyInjectionApp.Infrastructure.DTOs;

public class OpenMeteoResponse
{
    public CurrentWeatherData? CurrentWeather { get; set; }

    public class CurrentWeatherData
    {
        public double Temperature { get; set; }
        public double Windspeed { get; set; }
    }
} 