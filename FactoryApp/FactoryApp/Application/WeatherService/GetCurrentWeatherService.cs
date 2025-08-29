using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Presentation.DTOs;

namespace FactoryApp.Application.UseCases;

public class GetCurrentWeatherService: IWeatherService
{
    private readonly IWeatherClient _weatherClient;

    public GetCurrentWeatherService(IWeatherClient weatherClient)
    {
        _weatherClient = weatherClient;
    }
    public async Task<WeatherResponseDTO?> GetCurrentWeatherAsync(WeatherRequest request)
    {
        var response = await _weatherClient.GetCurrentWeatherAsync(request);

        if (response == null)
            return null;

        return new WeatherResponseDTO
        {
            Latitude = response.Latitude,
            Longitude = response.Longitude,
            Timezone = response.Timezone,
            CurrentWeather = new CurrentWeatherDTO
            {
                Time = response.Current_weather.Time,
                Temperature = response.Current_weather.Temperature,
                Windspeed = response.Current_weather.Windspeed,
                Winddirection = response.Current_weather.Winddirection,
                IsDay = response.Current_weather.Is_day,
                Weathercode = response.Current_weather.Weathercode
            }
        };
    }
} 