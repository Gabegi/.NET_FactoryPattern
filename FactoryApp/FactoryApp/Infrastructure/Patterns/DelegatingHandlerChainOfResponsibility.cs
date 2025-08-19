namespace FactoryApp.Infrastructure.Patterns
{
    // 3. CHAIN OF RESPONSIBILITY PATTERN - The handlers themselves!
    public abstract class DelegatingHandlerChainOfResponsibility
    {
        // Each handler in the chain has a reference to the next handler
        public HttpMessageHandler? InnerHandler { get; set; }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Each handler can:
            // 1. Handle the request itself (like caching)
            // 2. Pass it to the next handler in the chain
            // 3. Modify the request/response as it passes through

            return await base.SendAsync(request, cancellationToken); // Pass to next handler
        }
    }
}
