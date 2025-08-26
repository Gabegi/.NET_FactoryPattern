using FactoryApp.Application.WeatherService;
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

        public HttpClient CreateBaseClient(WeatherRequest request)
        {
            _logger.LogInformation($"Creating WeatherClient for {request.ServiceName} ({request.Region}, {request.Environment})");

            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri("");
            httpClient.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);

            return httpClient;
        }

    }
}

