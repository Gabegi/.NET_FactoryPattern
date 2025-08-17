using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherServiceFactory _weatherServiceFactory;

    public GetCurrentWeatherUseCase(IWeatherServiceFactory weatherServiceFactory)
    {
        _weatherServiceFactory = weatherServiceFactory;
    }

    public async Task<Weather?> ExecuteAsync(string serviceName = "openmeteo")
    {
        //// Create a simple request for backward compatibility
        //var request = new WeatherServiceCreationRequest
        //{
        //    Environment = "production",
        //    Region = "global",
        //    EnableCaching = true,
        //    EnableRetryPolicy = true,
        //    RequiredFeatures = new List<string> { "current_weather" }
        //};

        var weatherService = _weatherServiceFactory.CreateWeatherService(serviceName);
        return await weatherService.GetCurrentWeatherAsync();
    }

    public async Task<Weather?> ExecuteAsync(double latitude, double longitude, WeatherServiceCreationRequest request)
    {
        var weatherService = _weatherServiceFactory.CreateWeatherService(request);
        return await weatherService.GetCurrentWeatherAsync(latitude, longitude);
    }
} 