using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Patterns
{
    public class HttpHandlerChainBuilder
    {
        private readonly ILogger<HttpHandlerChainBuilder> _logger;

        public HttpHandlerChainBuilder(ILogger<HttpHandlerChainBuilder> logger)
        {
            _logger = logger;
        }
        public HttpClient BuildClientWithHandlers(HttpClient baseClient, HttpClientFeatures features, string serviceName)
        {
            var handlers = new List<DelegatingHandler>();

            // Build handler chain based on enabled features
            // Note: Order matters! Outermost handler is added last

            if (features.EnableCaching)
            {
                var cachingHandler = CreateCachingHandler(features, serviceName);
                handlers.Add(cachingHandler);
                _logger.LogDebug("Added caching handler for {ServiceName}", serviceName);
            }

            if (features.EnableRetry)
            {
                var retryHandler = CreateRetryHandler(features, serviceName);
                handlers.Add(retryHandler);
                _logger.LogDebug("Added retry handler for {ServiceName}", serviceName);
            }

            if (features.EnableAuth && !string.IsNullOrEmpty(features.AuthType))
            {
                var authHandler = CreateAuthHandler(features, serviceName);
                handlers.Add(authHandler);
                _logger.LogDebug("Added auth handler ({AuthType}) for {ServiceName}", features.AuthType, serviceName);
            }

            if (features.EnableLogging)
            {
                var loggingHandler = CreateLoggingHandler(serviceName);
                handlers.Add(loggingHandler);
                _logger.LogDebug("Added logging handler for {ServiceName}", serviceName);
            }

            if (features.EnableRateLimiting)
            {
                var rateLimitHandler = CreateRateLimitHandler(features, serviceName);
                handlers.Add(rateLimitHandler);
                _logger.LogDebug("Added rate limiting handler for {ServiceName}", serviceName);
            }

            // If no handlers, return the base client
            if (handlers.Count == 0)
            {
                return baseClient;
            }

            return CreateClientWithHandlerChain(baseClient, handlers); // Final build step
        }

        private HttpClient CreateClientWithHandlerChain(HttpClient baseClient, List<DelegatingHandler> handlers)
        {
            // This is the method that actually builds the Chain of Responsibility!

            if (handlers.Count == 0)
            {
                // No handlers to chain, return base client as-is
                return baseClient;
            }

            // Get the base handler from the HttpClient
            var baseHandler = GetHttpClientHandler(baseClient);

            // Chain handlers together: each handler's InnerHandler points to the next
            DelegatingHandler previousHandler = null;

            foreach (var handler in handlers)
            {
                if (previousHandler == null)
                {
                    // First handler connects directly to the base HTTP handler
                    handler.InnerHandler = baseHandler;
                }
                else
                {
                    // Subsequent handlers connect to the previous handler
                    handler.InnerHandler = previousHandler;
                }
                previousHandler = handler;
            }

            // The chain is now built. Example with 3 handlers:
            // handlers[0].InnerHandler = baseHandler
            // handlers[1].InnerHandler = handlers[0] 
            // handlers[2].InnerHandler = handlers[1]
            // 
            // So the chain flows: handlers[2] → handlers[1] → handlers[0] → baseHandler

            // Create new HttpClient using the OUTERMOST handler as the entry point
            var outermostHandler = handlers.LastOrDefault() ?? (HttpMessageHandler)baseHandler;
            var chainedClient = new HttpClient(outermostHandler, disposeHandler: true);

            // Copy configuration from the original base client
            CopyClientConfiguration(baseClient, chainedClient);

            // Dispose the old base client since we've extracted what we need
            baseClient.Dispose();

            return chainedClient; // This client now has ALL handlers working together
        }

        private HttpMessageHandler GetHttpClientHandler(HttpClient client)
        {
            // Use reflection to get the underlying handler
            var handlerField = typeof(HttpClient).GetField("_handler",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return (HttpMessageHandler)handlerField?.GetValue(client) ?? new HttpClientHandler();
        }

        private void CopyClientConfiguration(HttpClient source, HttpClient destination)
        {
            destination.BaseAddress = source.BaseAddress;
            destination.Timeout = source.Timeout;

            // Copy headers
            foreach (var header in source.DefaultRequestHeaders)
            {
                destination.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

    }
}
