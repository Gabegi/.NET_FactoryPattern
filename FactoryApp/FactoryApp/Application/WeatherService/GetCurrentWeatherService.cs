using FactoryApp.Application.WeatherService;
using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherService
{
    private readonly IWeatherClient _weatherClient;

    public GetCurrentWeatherService(IWeatherClient weatherClient)
    {
        _weatherClient = weatherClient;
    }

    // TO DO: Add mapping + validating + other logic here
    // TO DO: Use WeatherRequestDTO
    public async Task<WeatherResponse?> GetWeatherAsync(WeatherRequestDTO request)
    {
        return await _weatherClient.GetCurrentWeatherAsync(request);

    }
} 