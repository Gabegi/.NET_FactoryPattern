using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Domain.Extensions;

namespace FactoryApp.Infrastructure.Handlers
{
    public class BaseClientHandler : IBaseClient
    {
        private readonly ILogger<BaseClientHandler> _logger;
        private readonly ServiceCollection _services;

        public BaseClientHandler(
        ILogger<BaseClientHandler> logger,
           ServiceCollection services)
        {
            _logger = logger;
            _services = services;
        }

        public IHttpClientBuilder CreateBaseClient(WeatherRequest request)
        {
            _logger.LogInformation($"Creating Client Builder for {request.ServiceName} ({request.Region}, {request.Environment})");

            return _services.AddHttpClient($"{request.ServiceName}_client", client =>
            {
                ConfigureBaseClient(client, request);
            });
        }

        private void ConfigureBaseClient(HttpClient client, WeatherRequest request)
        {
            var serviceConfig = request.ServiceType.GetServiceConfig();

            if (serviceConfig == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig), "Service configuration cannot be null.");
            }

            if (serviceConfig.Url == null)
            {
                throw new ArgumentNullException(nameof(serviceConfig.Url), "Service URL cannot be null.");
            }

            client.BaseAddress = new Uri(serviceConfig.Url);
            client.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);
        }
    }
}

