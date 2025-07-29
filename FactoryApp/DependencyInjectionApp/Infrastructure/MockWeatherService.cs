using DependencyInjectionApp.Application.Interfaces;
using DependencyInjectionApp.Controllers.DTOs;

namespace DependencyInjectionApp.Infrastructure;

public class MockWeatherService : IWeatherService
{
    public async Task<WeatherInfo?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        // Simulate network delay
        await Task.Delay(100);
        
        // Return mock data
        return new WeatherInfo
        {
            Temperature = 22.5,
            WindSpeed = 5.2,
            Units = "°C"
        };
    }
} 