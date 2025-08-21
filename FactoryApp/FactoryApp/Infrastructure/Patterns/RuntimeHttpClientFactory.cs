using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.Http;

namespace FactoryApp.Infrastructure.Patterns
{

        public class RuntimeHttpClientFactory : IRuntimeHttpClientFactory
        {
            private readonly IBaseClient _baseClient;
            private readonly IServiceProvider _sp;

            public RuntimeHttpClientFactory(
                IBaseClient baseClient, 
                IServiceProvider sp)
            {
                _baseClient = baseClient;
                _sp = sp;
            }

            public HttpClient Create(HttpClientFeatures features, WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
            {
                // Always start with a clean client
                var client = _baseClient.CreateBaseClient(request, config);

                var handlers = new List<DelegatingHandler>();

                if (features.EnableCaching)
                    handlers.Add(ActivatorUtilities.CreateInstance<CachingHandler>(_sp));

                if (features.EnableRetry)
                    handlers.Add(ActivatorUtilities.CreateInstance<RetryHandler>(_sp));

                if (features.EnableAuth)
                    handlers.Add(ActivatorUtilities.CreateInstance<AuthHandler>(_sp));

                if (features.EnableLogging)
                    handlers.Add(ActivatorUtilities.CreateInstance<LoggingHandler>(_sp));

                if (features.EnableRateLimiting)
                    handlers.Add(ActivatorUtilities.CreateInstance<RateLimitHandler>(_sp));

                if (!handlers.Any())
                    return client; // plain client

                // 💡 Let HttpClientFactory build a proper pipeline
                var pipeline = HttpMessageHandlerBuilder.Build(handlers);

                return new HttpClient(pipeline, disposeHandler: true)
                {
                    BaseAddress = client.BaseAddress,
                    Timeout = client.Timeout
                };
            }
        }
}
