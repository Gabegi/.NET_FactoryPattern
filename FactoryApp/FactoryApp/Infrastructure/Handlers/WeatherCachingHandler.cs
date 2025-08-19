namespace FactoryApp.Infrastructure.Handlers
{
    public class WeatherCachingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Chain of Responsibility in action:
            var cacheKey = $"weather:{request.RequestUri}";
            var cached = await _cache.GetAsync(cacheKey);

            if (cached != null)
            {
                // This handler handles the request (cache hit)
                return new HttpResponseMessage { Content = new ByteArrayContent(cached) };
            }

            // Pass to next handler in chain (cache miss)
            var response = await base.SendAsync(request, cancellationToken);

            // Process response on the way back
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsByteArrayAsync();
                await _cache.SetAsync(cacheKey, content);
            }

            return response;
        }

    }
}
