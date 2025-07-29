using DependencyInjectionApp.Domain.Entities;
using DependencyInjectionApp.Domain.Interfaces;

namespace DependencyInjectionApp.Infrastructure.Services;

public class MockWeatherRepository : IWeatherRepository
{
    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        // Simulate network delay
        await Task.Delay(100);
        
        // Return mock data
        return new Weather(22.5, 5.2);
    }
} 