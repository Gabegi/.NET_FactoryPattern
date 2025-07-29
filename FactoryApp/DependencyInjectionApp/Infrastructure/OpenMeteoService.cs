using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.DTOs;
using DependencyInjectionApp.Infrastructure.Interfaces;
using static DependencyInjectionApp.Infrastructure.DTOs.WeatherResponse;

namespace DependencyInjectionApp.Infrastructure;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherResponse?> IWeatherService.GetCurrentWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<WeatherResponse>(url);
        if (response?.Temperature == null) return null;

        return response;
    }
} 