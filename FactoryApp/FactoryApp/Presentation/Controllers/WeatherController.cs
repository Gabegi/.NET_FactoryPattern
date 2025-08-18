using Microsoft.AspNetCore.Mvc;
using FactoryApp.Application.UseCases;
using static FactoryApp.Domain.Entities.WeatherServiceTypes;

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
            var response = await _getCurrentWeatherUseCase.ExecuteAsync(request);
            if (weather == null)
                return NotFound("Could not fetch weather.");



            return Ok(response);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
} 