using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherService _weatherService;

    public GetCurrentWeatherUseCase(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    // TO DO: Add mapping + validating + other logic here
    // TO DO: Use WeatherRequestDTO
    public async Task<Weather?> ExecuteAsync(string serviceName = "openmeteo")
    {
        return await _weatherService.GetCurrentWeatherAsync(serviceName);

        // TO DO: Mapping
        //return await weatherService.GetCurrentWeatherAsync();
    }
} 