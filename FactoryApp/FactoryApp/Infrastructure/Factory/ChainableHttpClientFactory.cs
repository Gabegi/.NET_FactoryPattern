using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Handlers;
using FactoryApp.Infrastructure.Interfaces;

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

            // Build the service provider and create the client
            var tempServiceProvider = _services.BuildServiceProvider();
            var factory = tempServiceProvider.GetRequiredService<IHttpClientFactory>();

            return factory.CreateClient($"{request.ServiceName}_client");
        }



        //private List<IHttpClientConfigurator> GetActiveConfigurators(WeatherRequest request)
        //{
        //    var activeConfigurators = new List<IHttpClientConfigurator>();

        //    // Order matters - logging should be outermost, then caching, then resilience
        //    if (request.EnableLogging)
        //        activeConfigurators.Add(configurators["logging"]);

        //    if (request.EnableCaching && _configurators.ContainsKey("caching"))
        //        activeConfigurators.Add(_configurators["caching"]);

        //    if (request.EnableRetryPolicy && _configurators.ContainsKey("resilience"))
        //        activeConfigurators.Add(_configurators["resilience"]);

        //    return activeConfigurators;
        //}
    }
}
