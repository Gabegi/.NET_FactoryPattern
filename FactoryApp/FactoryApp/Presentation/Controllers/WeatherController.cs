using FactoryApp.Application.UseCases;
using FactoryApp.Domain.Entities;
using FactoryApp.Presentation.DTOs;
using Microsoft.AspNetCore.Mvc;

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
        [FromQuery] WeatherServiceType serviceType)
    {
        try
        {
            var request = new WeatherRequestDTO
            {
                ServiceName = serviceType.,
            };

            var response = await _getCurrentWeatherUseCase.GetWeatherAsync(request);
            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning("Invalid request: {Message}", ex.Message);
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get weather for service: {ServiceName}", serviceName);
            return StatusCode(500, new { error = "Internal server error occurred" });
        }
    }
} 