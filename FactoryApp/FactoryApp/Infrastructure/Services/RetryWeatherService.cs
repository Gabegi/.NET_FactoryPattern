using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Services;

public class RetryWeatherService : IWeatherClient
{
    private readonly IWeatherClient _weatherService;
    private readonly IRetryPolicyService _retryService;

    public RetryWeatherService(IWeatherClient weatherService, IRetryPolicyService retryService)
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
