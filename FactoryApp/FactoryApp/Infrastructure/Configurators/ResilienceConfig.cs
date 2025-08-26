using FactoryApp.Application;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Configurators
{
    public class ResilienceConfig : IHttpClientConfigurator
    {
        public void Configure(
            IHttpClientBuilder clientBuilder,
            IServiceCollection services,
            WeatherClientCreationRequest request,
            WeatherServiceConfigAttribute config)

        {
            clientBuilder.AddStandardResilienceHandler(options =>
            {
                options.TotalRequestTimeout.Timeout = TimeSpan.FromSeconds(60);
                options.Retry.MaxRetryAttempts = 5;
                options.Retry.Delay = TimeSpan.FromMilliseconds(0);
                options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(5);
                options.CircuitBreaker.MinimumThroughput = 5;
                options.CircuitBreaker.FailureRatio = 0.9;
                options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(5);
                options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(1);
            });


        }


    }
}
