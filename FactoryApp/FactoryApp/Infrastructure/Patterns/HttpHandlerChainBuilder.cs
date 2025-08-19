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

        private HttpClient CreateClientWithHandlerChain(HttpClient baseClient, List<DelegatingHandler> handlers)
        {
            // Start with the innermost handler (the one HttpClient will actually call)
            HttpMessageHandler pipeline = baseClient?.Handler ?? new HttpClientHandler();

            // Handlers should be added in reverse order: the last one in the list should be the closest to the base handler
            for (int i = handlers.Count - 1; i >= 0; i--)
            {
                handlers[i].InnerHandler = pipeline;
                pipeline = handlers[i];
            }

            // Now build the HttpClient with the composed handler pipeline
            var client = new HttpClient(pipeline, disposeHandler: true)
            {
                BaseAddress = baseClient?.BaseAddress,
                Timeout = baseClient?.Timeout ?? TimeSpan.FromSeconds(100)
            };

            // Optionally copy headers or other config from baseClient
            if (baseClient != null)
            {
                foreach (var header in baseClient.DefaultRequestHeaders)
                {
                    client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            return client;
        }

    }
}
