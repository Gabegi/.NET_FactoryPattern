using AbstractFactoryApp.Domain.Entities;
using AbstractFactoryApp.Infrastructure.Interfaces;

namespace AbstractFactoryApp.Infrastructure.Services;

public class RetryWeatherService : IWeatherService
{
    private readonly IWeatherService _weatherService;
    private readonly IRetryPolicyService _retryService;

    public RetryWeatherService(IWeatherService weatherService, IRetryPolicyService retryService)
    {
        _weatherService = weatherService;
        _retryService = retryService;
    }

    public async Task<Weather?> GetCurrentWeatherAsync(double latitude, double longitude)
    {
        return await _retryService.ExecuteWithRetryAsync(async () =>
            await _weatherService.GetCurrentWeatherAsync(latitude, longitude));
    }

    public bool SupportsFeature(string feature)
    {
        return _weatherService.SupportsFeature(feature);
    }
}
