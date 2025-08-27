using FactoryApp.Application.WeatherService;
using FactoryApp.Infrastructure.Interfaces;

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

            return _services.AddHttpClient($"{request.ServiceName}", client =>
            {
                ConfigureBaseClient(client, request);
            });
        }

        private void ConfigureBaseClient(HttpClient client, WeatherRequest request)
        {
            if (!string.IsNullOrEmpty(request.ServiceName))
            {
                _logger.LogInformation($"Service name is empty, defaulting to Weather in Ouagadougou");
                client.BaseAddress = new Uri("https://api.open-meteo.com/v1/forecast?latitude=50&longitude=70&current_weather=true");
            }
                
            else
            {

            }

                client.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds);
        }
    }
}

