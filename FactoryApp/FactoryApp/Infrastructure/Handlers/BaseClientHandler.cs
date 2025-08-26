using FactoryApp.Application;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Handlers
{
    public class BaseClientHandler : IBaseClient
    {
        private readonly ILogger<BaseClientHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseClientHandler(
        ILogger<BaseClientHandler> logger,
        IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public HttpClient CreateBaseClient(WeatherClientCreationRequest request, WeatherServiceConfigAttribute config)
        {
            _logger.LogInformation($"Creating WeatherClient for {request.ServiceName} ({request.Region}, {request.Environment})");

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri(config.BaseUrl);
            httpClient.Timeout = TimeSpan.FromSeconds(config.TimeoutSeconds);

            return httpClient;
        }

    }
}

