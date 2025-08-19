using FactoryApp.Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System.Net;

namespace FactoryApp.Infrastructure.Config
{
    public class CachingService
    {
        // 1. Caching Handler
        public class ConditionalCachingHandler : DelegatingHandler
        {
            private readonly HybridCache _cache;
            private readonly ILogger<ConditionalCachingHandler> _logger;
            private readonly bool _cachingEnabled;
            private readonly TimeSpan _cacheDuration;

            public ConditionalCachingHandler(HybridCache cache,
                ILogger<ConditionalCachingHandler> logger,
                bool cachingEnabled = false,
                TimeSpan? cacheDuration = null)
            {
                _cache = cache;
                _logger = logger;
                _cachingEnabled = cachingEnabled;
                _cacheDuration = cacheDuration ?? TimeSpan.FromMinutes(5);
            }

            protected override async Task<HttpResponseMessage> SendAsync(
                HttpRequestMessage request, CancellationToken cancellationToken)
            {
                if (!_cachingEnabled || request.Method != HttpMethod.Get)
                {
                    return await base.SendAsync(request, cancellationToken);
                }

                var cacheKey = $"httpclient:{request.Method}:{request.RequestUri}";

                var cachedResponse = await _cache.GetOrCreateAsync(cacheKey, async _ =>
                {
                    _logger.LogDebug("Cache miss for {RequestUri}", request.RequestUri);
                    var response = await base.SendAsync(request, cancellationToken);

                    if (response.IsSuccessStatusCode)
                    {
                        var content = await response.Content.ReadAsByteArrayAsync();
                        return new CachedHttpResponse
                        {
                            StatusCode = response.StatusCode,
                            Content = content,
                            ContentType = response.Content.Headers.ContentType?.ToString(),
                            Headers = response.Headers.ToDictionary(h => h.Key, h => h.Value.ToArray())
                        };
                    }
                    return null;
                },
                new HybridCacheEntryOptions { Expiration = _cacheDuration });

                if (cachedResponse != null)
                {
                    _logger.LogDebug("Cache hit for {RequestUri}", request.RequestUri);
                    return CreateHttpResponseFromCache(cachedResponse);
                }

                return await base.SendAsync(request, cancellationToken);
            }

            private static HttpResponseMessage CreateHttpResponseFromCache(CachedHttpResponse cached)
            {
                var response = new HttpResponseMessage(cached.StatusCode)
                {
                    Content = new ByteArrayContent(cached.Content)
                };

                if (!string.IsNullOrEmpty(cached.ContentType))
                {
                    response.Content.Headers.ContentType = MediaTypeHeaderValue.Parse(cached.ContentType);
                }

                foreach (var header in cached.Headers)
                {
                    response.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                return response;
            }
        }

        public class CachedHttpResponse
        {
            public HttpStatusCode StatusCode { get; set; }
            public byte[] Content { get; set; } = Array.Empty<byte>();
            public string? ContentType { get; set; }
            public Dictionary<string, string[]> Headers { get; set; } = new();
        }

        // Configuration for HttpClient Handler approach
        public class WeatherClientSettings
        {
            public string BaseUrl { get; set; } = "";
            public bool EnableCaching { get; set; } = false;
            public int CacheDurationMinutes { get; set; } = 5;
        }

        // DI Configuration for HttpClient Handler
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHybridCache();
            services.Configure<WeatherClientSettings>(Configuration.GetSection("WeatherClient"));

            services.AddHttpClient<WeatherService>("WeatherClient", (serviceProvider, client) =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<WeatherClientSettings>>().Value;
                client.BaseAddress = new Uri(settings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(30);
            })
            .AddHttpMessageHandler(serviceProvider =>
            {
                var settings = serviceProvider.GetRequiredService<IOptions<WeatherClientSettings>>().Value;
                var cache = serviceProvider.GetRequiredService<HybridCache>();
                var logger = serviceProvider.GetRequiredService<ILogger<ConditionalCachingHandler>>();

                return new ConditionalCachingHandler(
                    cache,
                    logger,
                    settings.EnableCaching,
                    TimeSpan.FromMinutes(settings.CacheDurationMinutes));
            });
        }

        // Your CustomiseHttpClient method for Handler approach
        private HttpClient CustomiseHttpClient(HttpClient client, WeatherClientCreationRequest request)
        {
            if (request.EnableCaching)
            {
                _logger.LogDebug("Applied caching decorator - caching configured at HttpClient level");
            }

            client.DefaultRequestHeaders.Add("User-Agent", "WeatherApp/1.0");
            return client;
        }

        // Service for HttpClient Handler approach
        public class WeatherService
        {
            private readonly HttpClient _httpClient;
            private readonly ILogger<WeatherService> _logger;

            public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger)
            {
                _httpClient = httpClient; // Already has caching configured
                _logger = logger;
            }

            public async Task<WeatherResponse> GetWeatherAsync(string location)
            {
                // Caching happens automatically in the HttpClient handler
                var response = await _httpClient.GetAsync($"/weather?location={location}");
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<WeatherResponse>();
            }
        }
    }
}
