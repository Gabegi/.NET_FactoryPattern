using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Patterns
{
    public class ClientFactory : IClientFactory
    {
        private readonly IBaseClientHandler _baseClient;
        private readonly IHttpClientFactory _httpClientFactory;

        public ClientFactory(
            IBaseClientHandler baseClient,
            IHttpClientFactory httpClientFactory)
        {
            _baseClient = baseClient;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient Create(WeatherRequest request)
        {
            var clientName = $"{request.ServiceName}_client";

            // Start with base client configuration
            var clientBuilder = _baseClient.CreateBaseClient(request);

            if (request.EnableLogging is true)
            {
                clientBuilder.AddHttpMessageHandler<LoggingHandler>();
            }

            if (request.EnableCaching is true)
            {
                clientBuilder.AddHttpMessageHandler<CachingHandler>();
            }

            if (request.EnableResilience)
            {
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

            // Use the injected factory to create the client
            return _httpClientFactory.CreateClient(clientName);
        }
    }
}