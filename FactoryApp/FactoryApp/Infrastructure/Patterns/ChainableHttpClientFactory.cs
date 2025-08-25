using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Configurators;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Patterns
{
    // Main factory implementation
    public class ChainableHttpClientFactory : IChainableHttpClientFactory
    {
        private readonly IBaseClient _baseClient;
        private readonly Dictionary<string, IHttpClientConfigurator> _configurators;

        public ChainableHttpClientFactory(
            IBaseClient baseClient)
        {
            _baseClient = baseClient;
            _configurators = new Dictionary<string, IHttpClientConfigurator>
            {
                ["resilience"] = new ResilienceConfig()
                //["caching"] = new CachingConfigurator(),
                //["logging"] = new LoggingConfigurator()
            };
        }

        public HttpClient Create(HttpClientFeatures features, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
        {
            // Create a temporary service collection for dynamic client configuration
            var services = new ServiceCollection();

            // Start with base client configuration
            var clientBuilder = services.AddHttpClient("DynamicClient", client =>
            {
                ConfigureBaseClient(client, request, config);
            });

            // Chain configurations based on features
            var activeConfigurators = GetActiveConfigurators(features);

            foreach (var configurator in activeConfigurators)
            {
                configurator.Configure(clientBuilder, services, request, config);
            }

            // Build the service provider and create the client
            var tempServiceProvider = services.BuildServiceProvider();
            var factory = tempServiceProvider.GetRequiredService<IHttpClientFactory>();

            return factory.CreateClient("DynamicClient");
        }

        private void ConfigureBaseClient(HttpClient client, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
        {
            if (!string.IsNullOrEmpty(config.BaseUrl))
                client.BaseAddress = new Uri(config.BaseUrl);

            //if (!string.IsNullOrEmpty(request.ApiKey))
            //    client.DefaultRequestHeaders.Add("X-API-Key", request.ApiKey);

            client.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);
        }

        private List<IHttpClientConfigurator> GetActiveConfigurators(HttpClientFeatures features)
        {
            var activeConfigurators = new List<IHttpClientConfigurator>();

            // Order matters - logging should be outermost, then caching, then resilience
            //if (features.EnableLogging && _configurators.ContainsKey("logging"))
            //    activeConfigurators.Add(_configurators["logging"]);

            if (features.EnableCaching && _configurators.ContainsKey("caching"))
                activeConfigurators.Add(_configurators["caching"]);

            if (features.EnableRetry && _configurators.ContainsKey("resilience"))
                activeConfigurators.Add(_configurators["resilience"]);

            return activeConfigurators;
        }
    }
}
