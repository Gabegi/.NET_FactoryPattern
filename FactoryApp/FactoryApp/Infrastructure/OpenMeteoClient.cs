using FactoryApp.Application.WeatherService;
using FactoryApp.Domain;
using FactoryApp.Infrastructure.Interfaces;
using System.Text.Json;

namespace FactoryApp.Infrastructure;

public class OpenMeteoClient : IWeatherClient
{
    private readonly IClientFactory _clientFactory;
    private readonly ILogger<OpenMeteoClient> _logger;

    public OpenMeteoClient(
        IClientFactory clientFactory,
        ILogger<OpenMeteoClient> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(WeatherRequest request)
    {
        _logger.LogInformation($"Fetching weather data from Open-Meteo API for service {request.ServiceName}, environment {request.Environment}");

        // Create HttpClient using the factory with all configured handlers
        using var httpClient = _clientFactory.Create(request);

        try
        {
            var response = await httpClient.GetAsync();
            response.EnsureSuccessStatusCode();

            var jsonString = await response.Content.ReadAsStringAsync();
            var weather = JsonSerializer.Deserialize<Weather>(jsonString, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _logger.LogInformation($"Successfully retrieved weather data for service {request.ServiceName}");
            return weather;
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Error fetching weather data from Open-Meteo API for coordinates ({Latitude}, {Longitude})",
                request.Latitude, request.Longitude);
            return null;
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Error deserializing weather data for coordinates ({Latitude}, {Longitude})",
                request.Latitude, request.Longitude);
            return null;
        }
    }