using Microsoft.AspNetCore.Mvc;
using FactoryApp.Application.UseCases;
using FactoryApp.Presentation.DTOs;
using FactoryApp.Infrastructure.Factories;

namespace FactoryApp.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly GetCurrentWeatherUseCase _getCurrentWeatherUseCase;

    public WeatherController(GetCurrentWeatherUseCase getCurrentWeatherUseCase)
    {
        _getCurrentWeatherUseCase = getCurrentWeatherUseCase;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] string service = "tokyoDEV")
    {
        try
        {
            var weather = await _getCurrentWeatherUseCase.ExecuteAsync(service);
            if (weather == null)
                return NotFound("Could not fetch weather.");

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

            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 