using DependencyInjectionApp.Application.Interfaces;
using DependencyInjectionApp.Controllers.DTOs;

namespace DependencyInjectionApp.Infrastructure;

public class OpenMeteoService : IWeatherRepository
{
    private readonly HttpClient _httpClient;

    public OpenMeteoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherInfo?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);
        if (response?.CurrentWeather == null) return null;

        return new WeatherInfo
        {
            Temperature = response.CurrentWeather.Temperature,
            WindSpeed = response.CurrentWeather.Windspeed
        };
    }

    private class OpenMeteoResponse
    {
        public CurrentWeatherData? CurrentWeather { get; set; }

        public class CurrentWeatherData
        {
            public double Temperature { get; set; }
            public double Windspeed { get; set; }
        }
    }
} 