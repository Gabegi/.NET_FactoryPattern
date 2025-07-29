using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherService _weatherService;

    public GetCurrentWeatherUseCase(IWeatherService weatherRepository)
    {
        _weatherService = weatherRepository;
    }

    public async Task<Weather?> ExecuteAsync(double latitude, double longitude)
    {
        
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees");

        var dto = await _weatherService.GetCurrentWeatherAsync(latitude, longitude);

        return new Weather
        {
            Temperature = dto.Temperature,
            Timestamp = dto.Timestamp,
            Units = dto.Units,
            WindSpeed = dto.WindSpeed
        };
    }
} 