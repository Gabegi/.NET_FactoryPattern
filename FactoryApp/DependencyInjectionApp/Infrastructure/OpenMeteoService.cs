using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Infrastructure;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenMeteoService> _logger;
    private readonly IConfiguration _configuration;

    public OpenMeteoService(
        IHttpClientFactory httpClientFactory,
        ILogger<OpenMeteoService> logger,
        IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("WeatherApi");
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        string url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

        var response = await _httpClient.GetFromJsonAsync<Weather>(url);

        return response;
    }
} 