using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherServiceFactory _weatherServiceFactory;

    public GetCurrentWeatherUseCase(IWeatherServiceFactory weatherServiceFactory)
    {
        _weatherServiceFactory = weatherServiceFactory;
    }

    public async Task<Weather?> ExecuteAsync(double latitude, double longitude, string serviceType = "openmeteo")
    {
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees.");

        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees.");

        var weatherService = _weatherServiceFactory.CreateWeatherService(serviceType);
        return await weatherService.GetCurrentWeatherAsync(latitude, longitude);
    }
} 