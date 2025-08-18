using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using System.Text.Json;
using static FactoryApp.Domain.Entities.WeatherServiceTypes;

namespace FactoryApp.Infrastructure;

public class OpenMeteoService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IWeatherServiceFactory _weatherServiceFactory;
    private readonly ILogger<OpenMeteoService> _logger;


    public OpenMeteoService(
        IHttpClientFactory httpClientFactory,
        IWeatherServiceFactory weatherServiceFactory,
        ILogger<OpenMeteoService> logger)
    {
        _httpClient = httpClientFactory.CreateClient("WeatherApi");
        _weatherServiceFactory = weatherServiceFactory;
        _logger = logger;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(string serviceName)
    {
        _logger.LogInformation($"Fetching weather data from Open-Meteo API for service {serviceName}");

        try
        {
            var service = _weatherServiceFactory.CreateWeatherService(serviceName)



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

    public bool SupportsFeature(string feature)
    {
        return feature.ToLowerInvariant() switch
        {
            "current_weather" => true,
            "forecast" => true,
            "historical" => true,
            "hourly" => true,
            "daily" => true,
            _ => false
        };
    }
} 