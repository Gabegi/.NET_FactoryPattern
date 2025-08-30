using FactoryApp.Application.WeatherService;
using FactoryApp.Domain.Extensions;
using FactoryApp.Infrastructure.Interfaces;

public class BaseClientHandler : IBaseClientHandler
{
    private readonly ILogger<BaseClientHandler> _logger;
    private readonly IServiceCollection _services;

    public BaseClientHandler(
        ILogger<BaseClientHandler> logger,
        IServiceCollection services)
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

        if (string.IsNullOrEmpty(serviceConfig.Url))
            throw new ArgumentNullException(nameof(serviceConfig.Url), "Service URL cannot be null.");

        client.BaseAddress = new Uri(serviceConfig.Url +
            $"?latitude={serviceConfig.Latitude}&longitude={serviceConfig.Longitude}&current_weather=true");

        client.Timeout = TimeSpan.FromSeconds(request.CustomTimeoutSeconds > 0
            ? request.CustomTimeoutSeconds
            : serviceConfig.TimeoutSecond);
    }
}
