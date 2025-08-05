using Microsoft.AspNetCore.Mvc;
using KeyedDependencyInjectionApp.Application.UseCases;

namespace KeyedDependencyInjectionApp.Presentation.Controllers;

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
        [FromQuery] double lat = 51.5, 
        [FromQuery] double lon = -0.1,
        [FromQuery] string service = "openmeteo")
    {
        try
        {
            // Validate service key
            if (service != "openmeteo" && service != "mock")
            {
                return BadRequest("Service must be either 'openmeteo' or 'mock'");
            }

            var weather = await _getCurrentWeatherUseCase.ExecuteAsync(lat, lon, service);
            if (weather == null)
                return NotFound("Could not fetch weather.");

            return Ok(new
            {
                Service = service,
                Data = weather
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("mock")]
    public async Task<IActionResult> GetMock(
        [FromQuery] double lat = 51.5, 
        [FromQuery] double lon = -0.1)
    {
        try
        {
            var weather = await _getCurrentWeatherUseCase.ExecuteAsync(lat, lon, "mock");
            if (weather == null)
                return NotFound("Could not fetch weather.");

            return Ok(new
            {
                Service = "mock",
                Data = weather
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("openmeteo")]
    public async Task<IActionResult> GetOpenMeteo(
        [FromQuery] double lat = 51.5, 
        [FromQuery] double lon = -0.1)
    {
        try
        {
            var weather = await _getCurrentWeatherUseCase.ExecuteAsync(lat, lon, "openmeteo");
            if (weather == null)
                return NotFound("Could not fetch weather.");

            return Ok(new
            {
                Service = "openmeteo",
                Data = weather
            });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 