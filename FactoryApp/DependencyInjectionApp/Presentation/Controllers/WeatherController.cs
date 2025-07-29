using Microsoft.AspNetCore.Mvc;
using DependencyInjectionApp.Application.UseCases;
using DependencyInjectionApp.Domain.Entities;

namespace DependencyInjectionApp.Presentation.Controllers;

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
    public async Task<IActionResult> Get([FromQuery] double lat = 51.5, [FromQuery] double lon = -0.1)
    {
        try
        {
            var weather = await _getCurrentWeatherUseCase.ExecuteAsync(lat, lon);
            if (weather == null)
                return NotFound("Could not fetch weather.");

            return Ok(new
            {
                Temperature = weather.Temperature,
                WindSpeed = weather.WindSpeed,
                Units = weather.Units,
                Timestamp = weather.Timestamp,
                IsCold = weather.IsCold,
                IsWarm = weather.IsWarm,
                IsWindy = weather.IsWindy
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 