using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Configuration
{
    public class ResilienceConfigurator : IHttpClientConfigurator
    {
        public void Configure(IHttpClientBuilder clientBuilder, WeatherRequest request)
        {
            if (!request.EnableRetryPolicy) return;

            clientBuilder.AddStandardResilienceHandler(options =>
            {
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
                options.Retry.MaxRetryAttempts = 5;
                options.Retry.Delay = TimeSpan.Zero;
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(5);
                options.CircuitBreaker.MinimumThroughput = 5;
                options.CircuitBreaker.FailureRatio = 0.9;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(5);
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
            });
        }
    }
}
