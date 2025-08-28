using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Http.Resilience;

namespace FactoryApp.Infrastructure.Patterns
{
    // Main factory implementation
    public class ChainableHttpClientFactory : IChainableHttpClientFactory
    {
        private readonly IBaseClient _baseClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEnumerable<IHttpClientConfigurator> _configurators;
        ServiceCollection _services;



        public ChainableHttpClientFactory(
            IBaseClient baseClient,
            IServiceProvider serviceProvider,
            IHttpClientFactory httpClientFactory,
            IEnumerable<IHttpClientConfigurator> configurators)
        {
            _baseClient = baseClient;
            _serviceProvider = serviceProvider;
            _httpClientFactory = httpClientFactory;
            _configurators = configurators;
        }

        public HttpClient Create(WeatherRequest request)
        {

            // Start with base client configuration
            var clientBuilder = _baseClient.CreateBaseClient(request);

            //var configurations = GetActiveConfigurators(request);
            if(request.EnableLogging is true)
            {
                clientBuilder.AddHttpMessageHandler<LoggingHandler>();
            }

            if(request.EnableCaching is true)
            {
                clientBuilder.AddHttpMessageHandler<CachingHandler>();
            }

            if(request.EnableResilience)
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

            // Build the service provider and create the client
            var tempServiceProvider = _services.BuildServiceProvider();
            var factory = tempServiceProvider.GetRequiredService<IHttpClientFactory>();

            return factory.CreateClient($"{request.ServiceName}_client");
        }
    }
}
