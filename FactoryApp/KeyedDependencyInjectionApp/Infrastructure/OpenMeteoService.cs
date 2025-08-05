using KeyedDependencyInjectionApp.Domain.Entities;
using KeyedDependencyInjectionApp.Infrastructure.Interfaces;

namespace KeyedDependencyInjectionApp.Infrastructure;

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
        try
        {
            var apiUrl = _configuration["WeatherApi:BaseUrl"] ?? "https://api.open-meteo.com/v1";
            var url = $"{apiUrl}/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";

            _logger.LogInformation("Fetching weather data for coordinates: {Lat}, {Lon}", latitude, longitude);

            var response = await _httpClient.GetFromJsonAsync<Weather>(url);

            if (response?.Current_weather == null)
            {
                _logger.LogWarning("No weather data received for coordinates: {Lat}, {Lon}", latitude, longitude);
                return null;
            }

            return response;
            
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to fetch weather data");
            return null;
        }
    }
} 