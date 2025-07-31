using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;
using DependencyInjectionApp.Presentation.DTOs;
using static DependencyInjectionApp.Presentation.DTOs.WeatherResponseDTO;

namespace DependencyInjectionApp.Infrastructure;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _httpClient;

    public OpenMeteoService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient();
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<Weather>(url);

        return response;
    }
} 