using FactoryApp.Domain.Entities;

namespace FactoryApp.Infrastructure.Patterns
{
    public class HttpHandlerChainBuilder
    {
        public HttpClient BuildClientWithHandlers(HttpClient baseClient, HttpClientFeatures features, string serviceName)
        {
            var handlers = new List<DelegatingHandler>();

            // Builder pattern - conditionally adding components
            if (features.EnableCaching)
                handlers.Add(CreateCachingHandler(features, serviceName));

            if (features.EnableRetry)
                handlers.Add(CreateRetryHandler(features, serviceName));

            if (features.EnableAuth)
                handlers.Add(CreateAuthHandler(features, serviceName));

            if (features.EnableLogging)
                handlers.Add(CreateLoggingHandler(serviceName));

            return CreateClientWithHandlerChain(baseClient, handlers); // Final build step
        }
    }
}
