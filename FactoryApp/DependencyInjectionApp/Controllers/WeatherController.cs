using Microsoft.AspNetCore.Mvc;
using DependencyInjectionApp.Application.Interfaces;
using DependencyInjectionApp.Controllers.DTOs;

namespace DependencyInjectionApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        // Dependency injection - service is injected by the DI container
        _weatherService = weatherService;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] double lat = 51.5, [FromQuery] double lon = -0.1)
    {
        var result = await _weatherService.GetCurrentWeatherAsync(lat, lon);
        if (result == null)
            return NotFound("Could not fetch weather.");

        return Ok(result);
    }
} 