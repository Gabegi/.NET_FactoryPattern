using AbstractFactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;

namespace AbstractFactoryApp.Infrastructure.Services;

public class SimpleRetryPolicyService : IRetryPolicyService
{
    private readonly ILogger<SimpleRetryPolicyService> _logger;

    public SimpleRetryPolicyService(ILogger<SimpleRetryPolicyService> logger)
    {
        _logger = logger;
    }

    public async Task<T> ExecuteWithRetryAsync<T>(Func<Task<T>> operation, int maxRetries = 3)
    {
        var attempt = 0;
        while (true)
        {
            try
            {
                return await operation();
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                attempt++;
                _logger.LogWarning(ex, "Operation failed on attempt {Attempt}/{MaxRetries}, retrying...", attempt, maxRetries);
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Exponential backoff
            }
        }
    }

    public async Task ExecuteWithRetryAsync(Func<Task> operation, int maxRetries = 3)
    {
        var attempt = 0;
        while (true)
        {
            try
            {
                await operation();
                return;
            }
            catch (Exception ex) when (attempt < maxRetries)
            {
                attempt++;
                _logger.LogWarning(ex, "Operation failed on attempt {Attempt}/{MaxRetries}, retrying...", attempt, maxRetries);
                await Task.Delay(TimeSpan.FromSeconds(Math.Pow(2, attempt))); // Exponential backoff
            }
        }
    }
}
