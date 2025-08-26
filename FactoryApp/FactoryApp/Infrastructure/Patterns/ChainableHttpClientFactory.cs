using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Configurators;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Patterns
{
    // Main factory implementation
    public class ChainableHttpClientFactory : IChainableHttpClientFactory
    {
        private readonly IBaseClient _baseClient;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, IHttpClientConfigurator> _configurators;

        public ChainableHttpClientFactory(
            IBaseClient baseClient,
            IServiceProvider serviceProvider)
        {
            _baseClient = baseClient;
            _serviceProvider = serviceProvider;
            _configurators = new Dictionary<string, IHttpClientConfigurator>
            {
                ["resilience"] = new ResilienceConfig()
                //["caching"] = new CachingConfigurator(),
                //["logging"] = new LoggingConfigurator()
            };
        }

        public HttpClient Create(WeatherRequest request)
        {
            // Create a temporary service collection for dynamic client configuration
            var services = new ServiceCollection();

            // Start with base client configuration
            var clientBuilder = services.AddHttpClient("DynamicClient", client =>
            {
                ConfigureBaseClient(client, request);
            });

            // Chain configurations based on features
            var activeConfigurators = GetActiveConfigurators(request);

            foreach (var configurator in activeConfigurators)
            {
                configurator.Configure(clientBuilder, services, request);
            }

            // Build the service provider and create the client
            var tempServiceProvider = services.BuildServiceProvider();
            var factory = tempServiceProvider.GetRequiredService<IHttpClientFactory>();

            return factory.CreateClient("DynamicClient");
        }

        private void ConfigureBaseClient(HttpClient client, WeatherRequest request)
        {
            if (!string.IsNullOrEmpty(request.ServiceName))
                client.BaseAddress = new Uri("");

            //if (!string.IsNullOrEmpty(request.ApiKey))
            //    client.DefaultRequestHeaders.Add("X-API-Key", request.ApiKey);

            client.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);
        }

        private List<IHttpClientConfigurator> GetActiveConfigurators(WeatherRequest request)
        {
            var activeConfigurators = new List<IHttpClientConfigurator>();

            // Order matters - logging should be outermost, then caching, then resilience
            if (request.EnableLogging && _configurators.ContainsKey("logging"))
                activeConfigurators.Add(_configurators["logging"]);

            if (request.EnableCaching && _configurators.ContainsKey("caching"))
                activeConfigurators.Add(_configurators["caching"]);

            if (request.EnableRetryPolicy && _configurators.ContainsKey("resilience"))
                activeConfigurators.Add(_configurators["resilience"]);

            return activeConfigurators;
        }
    }
}
