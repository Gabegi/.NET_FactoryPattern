using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;
using System.Text.Json;

namespace FactoryApp.Infrastructure;

public class OpenMeteoClient : IWeatherClient
{
    private readonly HttpClient _httpClient;
    private readonly IWeatherClientFactory _weatherServiceFactory;
    private readonly ILogger<OpenMeteoClient> _logger;


    public OpenMeteoClient(
        IHttpClientFactory httpClientFactory,
        IWeatherClientFactory weatherServiceFactory,
        ILogger<OpenMeteoClient> logger)
    {
        _httpClient = httpClientFactory.CreateClient("WeatherApi");
        _weatherServiceFactory = weatherServiceFactory;
        _logger = logger;
    }

    public async Task<WeatherResponseDTO> GetForecastAsync(WeatherClientCreationRequest request)
    {
        var httpClient = CreateBaseClient(request);
        var url = _configuration["WeatherApi:Url"] ?? throw new InvalidOperationException("WeatherApi:Url not found in appsettings");

        var response = await httpClient.GetAsync(url);

        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStreamAsync();

        var result = JsonSerializer.Deserialize<WeatherResponseDTO?>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result ?? throw new InvalidOperationException("Failed to deserialize weather response - received null");
    }

    public async Task<Weather?> GetCurrentWeatherAsync(WeatherRequestDTO requestDTO)
    {
        _logger.LogInformation($"Fetching weather data from Open-Meteo API for service {requestDTO.ServiceName}, environment {requestDTO.Environment}");

        try
        {
            var request = new WeatherClientCreationRequest
            {
                ServiceName = requestDTO.ServiceName,
                Environment = requestDTO.Environment,

            };

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