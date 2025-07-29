using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Infrastructure;

public class OpenMeteoService : IWeatherService
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

    Task<Weather?> IWeatherService.GetCurrentWeatherAsync(double latitude, double longitude)
    {
        throw new NotImplementedException();
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