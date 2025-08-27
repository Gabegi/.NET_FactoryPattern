using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Handlers
{
    public class BaseClientHandler : IBaseClient
    {
        private readonly ILogger<BaseClientHandler> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ServiceCollection _services;

        public BaseClientHandler(
        ILogger<BaseClientHandler> logger,
        IHttpClientFactory httpClientFactory,
           ServiceCollection services)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _services = services;
        }

        public IHttpClientBuilder CreateBaseClient(WeatherRequest request)
        {
            _logger.LogInformation($"Creating Client Builder for {request.ServiceName} ({request.Region}, {request.Environment})");

            _services.AddHttpClient($"{request.ServiceName}", client =>
            {
                ConfigureBaseClient(client, request);
            });


            var httpClient = _httpClientFactory.CreateClient("WeatherApi");
            httpClient.BaseAddress = new Uri("");
            httpClient.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);

            return httpClient;
        }

        private void ConfigureBaseClient(HttpClient client, WeatherRequest request)
        {
            if (!string.IsNullOrEmpty(request.ServiceName))
                client.BaseAddress = new Uri("");

            //if (!string.IsNullOrEmpty(request.ApiKey))
            //    client.DefaultRequestHeaders.Add("X-API-Key", request.ApiKey);

            client.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);
        }
    }
}

