using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;

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
    public async Task<Weather?> GetWeatherAsync(WeatherRequestDTO request)
    {
        return await _weatherService.GetCurrentWeatherAsync(serviceName);

    }
} 