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
        var response = new WeatherResponseDTO
        {
            Latitude = weather.Latitude,
            Longitude = weather.Longitude,
            Timezone = weather.Timezone,
            CurrentWeather = new CurrentWeatherDTO
            {
                Time = weather.Current_weather.Time,
                Temperature = weather.Current_weather.Temperature,
                Windspeed = weather.Current_weather.Windspeed,
                Winddirection = weather.Current_weather.Winddirection,
                IsDay = weather.Current_weather.Is_day,
                Weathercode = weather.Current_weather.Weathercode
            }
        };
        //return await weatherService.GetCurrentWeatherAsync();
    }
} 