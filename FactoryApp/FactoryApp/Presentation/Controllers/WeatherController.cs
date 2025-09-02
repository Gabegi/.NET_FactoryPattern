using FactoryApp.Application.UseCases;
using FactoryApp.Application.WeatherService;
using FactoryApp.Domain;
using Microsoft.AspNetCore.Mvc;

namespace FactoryApp.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(
        IWeatherService currentWeather,
        ILogger<WeatherController> logger)
    {
        _weatherService = currentWeather;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] WeatherServiceType serviceType
        )
    {
        try
        {
            var request = new WeatherRequest
            {
                ClientName = serviceType.ToString(),
            };

            var response = await _weatherService.GetCurrentWeatherAsync(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid request: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Failed to get weather for service: {serviceType}");
            return StatusCode(500, new { error = "Internal server error occurred" });
        }
    }
} 