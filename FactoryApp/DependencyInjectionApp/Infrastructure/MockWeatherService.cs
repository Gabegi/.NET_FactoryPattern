using DependencyInjectionApp.Infrastructure.DTOs;
using DependencyInjectionApp.Infrastructure.Interfaces;

namespace DependencyInjectionApp.Infrastructure;

public class MockWeatherService : IWeatherService
{
    public async Task<WeatherResponse?> IWeatherService.GetCurrentWeatherAsync(double latitude, double longitude)
    {
        // Simulate network delay
        await Task.Delay(100);

        // Return mock data
        return new WeatherResponse
        {
            Temperature = 22.5,
            WindSpeed = 5.2,
            Units = "°C"
        };
    }
} 