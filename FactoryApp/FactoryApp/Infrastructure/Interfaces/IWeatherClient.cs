using FactoryApp.Application.WeatherService;
using FactoryApp.Domain;

namespace FactoryApp.Infrastructure.Interfaces;

public interface IWeatherClient
{
    Task<Weather?> GetCurrentWeatherAsync(WeatherRequest request);
} 