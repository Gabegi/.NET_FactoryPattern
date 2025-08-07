using System.Text.Json;
using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<OpenMeteoService> _logger;

    public OpenMeteoService(HttpClient httpClient, ILogger<OpenMeteoService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        try
        {
            var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&current_weather=true";
            
            _logger.LogInformation("Fetching weather data from Open-Meteo API for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            
            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            
            var jsonString = await response.Content.ReadAsStringAsync();
            var weather = JsonSerializer.Deserialize<Weather>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            
            _logger.LogInformation("Successfully retrieved weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            
            return weather;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching weather data from Open-Meteo API for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing weather data for coordinates: {Latitude}, {Longitude}", latitude, longitude);
            return null;
        }
    }
} 