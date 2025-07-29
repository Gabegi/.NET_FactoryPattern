using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Domain.Interfaces;
using DependencyInjectionApp.Infrastructure.DTOs;

namespace DependencyInjectionApp.Infrastructure.Services;

public class OpenMeteoWeatherRepository : IWeatherRepository
{
    private readonly HttpClient _httpClient;

    public OpenMeteoWeatherRepository(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<OpenMeteoResponse>(url);
        if (response?.CurrentWeather == null) return null;

        return new Weather(
            response.CurrentWeather.Temperature,
            response.CurrentWeather.Windspeed
        );
    }
} 