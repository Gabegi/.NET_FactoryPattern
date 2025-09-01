using FactoryApp.Application.WeatherService;
using FactoryApp.Domain.Extensions;
using FactoryApp.Infrastructure.Interfaces;

namespace FactoryApp.Infrastructure.Handlers
{
    public class BaseClientHandler : IBaseClientHandler
    {
        private readonly ILogger<BaseClientHandler> _logger;

        public BaseClientHandler(
            ILogger<BaseClientHandler> logger)
        {
            _logger = logger;
        }

        public void ConfigureBaseClient(HttpClient client, WeatherRequest request)
        {
            var serviceConfig = request.ServiceType.GetServiceConfig();

            if (string.IsNullOrEmpty(serviceConfig.Url))
                throw new ArgumentNullException(nameof(serviceConfig.Url), "Service URL cannot be null.");

            client.BaseAddress = new Uri(serviceConfig.Url +
                $"?latitude={serviceConfig.Latitude}&longitude={serviceConfig.Longitude}&current_weather=true");

            client.Timeout = TimeSpan.FromSeconds(serviceConfig.TimeoutSecond);
        }
    }
}
