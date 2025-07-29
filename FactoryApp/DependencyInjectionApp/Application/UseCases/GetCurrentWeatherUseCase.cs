using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Domain.Interfaces;

namespace DependencyInjectionApp.Application.UseCases;

public class GetCurrentWeatherUseCase
{
    private readonly IWeatherRepository _weatherRepository;

    public GetCurrentWeatherUseCase(IWeatherRepository weatherRepository)
    {
        _weatherRepository = weatherRepository;
    }

    public async Task<Weather?> ExecuteAsync(double latitude, double longitude)
    {
        // Application logic can be added here (validation, business rules, etc.)
        if (latitude < -90 || latitude > 90)
            throw new ArgumentException("Latitude must be between -90 and 90 degrees");
        
        if (longitude < -180 || longitude > 180)
            throw new ArgumentException("Longitude must be between -180 and 180 degrees");

        return await _weatherRepository.GetCurrentWeatherAsync(latitude, longitude);
    }
} 