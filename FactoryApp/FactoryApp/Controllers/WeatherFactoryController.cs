using Microsoft.AspNetCore.Mvc;
using WeatherApp.Application.Interfaces;
using WeatherApp.Infrastructure;

namespace WeatherApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherFactoryController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherFactoryController()
    {
        // Direct factory usage (for demo; in production use DI)
        _weatherService = WeatherServiceFactory.Create("openmeteo");
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
