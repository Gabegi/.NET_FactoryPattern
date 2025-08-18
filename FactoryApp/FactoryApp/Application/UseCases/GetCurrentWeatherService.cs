using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherService
{
    private readonly Infrastructure.Interfaces.IWeatherClient _weatherService;

    public GetCurrentWeatherService(IOpen weatherService)
    {
        _weatherService = weatherService;
    }

    // TO DO: Add mapping + validating + other logic here
    // TO DO: Use WeatherRequestDTO
    public async Task<Weather?> GetWeatherAsync(WeatherRequestDTO request)
    {
        return await _weatherService.GetCurrentWeatherAsync(request);

    }
} 