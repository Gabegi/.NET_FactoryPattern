using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Patterns
{
    public class ClientFactory : IClientFactory
    {
        private readonly IBaseClientHandler _baseClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpClientBuilder _clientBuilder;


        public ClientFactory(
            IBaseClientHandler baseClient,
            IHttpClientFactory httpClientFactory,
            IHttpClientBuilder httpClientBuilder)
        {
            _baseClient = baseClient;
            _httpClientFactory = httpClientFactory;
            _clientBuilder = httpClientBuilder;
        }

        public HttpClient Create(WeatherRequest request)
        {
            var clientName = $"{request.ServiceName}_client";

            // Start with base client configuration

            var httpClient = _httpClientFactory.CreateClient(clientName);
            _baseClient.ConfigureBaseClient(httpClient, request);

            if (request.EnableLogging is true)
            {
                _clientBuilder.AddHttpMessageHandler<LoggingHandler>();
            }

            if (request.EnableCaching is true)
            {
                _clientBuilder.AddHttpMessageHandler<CachingHandler>();
            }

            if (request.EnableResilience)
            {
                _clientBuilder.AddStandardResilienceHandler(options =>
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