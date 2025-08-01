﻿using FactoryApp.Controllers.DTOs;

namespace WeatherApp.Application.Interfaces;

public interface IWeatherService
{
    Task<WeatherInfo?> GetCurrentWeatherAsync(double latitude, double longitude);
}
