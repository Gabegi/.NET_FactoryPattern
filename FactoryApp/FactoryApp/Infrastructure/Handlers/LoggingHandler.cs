using FactoryApp.Application.WeatherService;
using FactoryApp.Domain;
using FactoryApp.Domain.Extensions;
using FactoryApp.Infrastructure.Interfaces;
using System.Diagnostics;

namespace FactoryApp.Infrastructure.Handlers
{
    public class LoggingHandler : IHttpClientConfigurator
    {
        public void Configure(
            IHttpClientBuilder clientBuilder,
            ServiceCollection services,
            WeatherRequest request)
        {
            // Ensure logging services are registered
            if (!services.Any(s => s.ServiceType == typeof(ILoggerFactory)))
            {
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddDebug();

                    // Different log levels based on environment
                    var config = request.ServiceType.GetServiceConfig();
                    var logLevel = config.Environment == "prod"
                        ? LogLevel.Information
                        : LogLevel.Debug;

                    builder.SetMinimumLevel(logLevel);
                });
            }

            // Add custom logging handler to HTTP pipeline
            clientBuilder.AddHttpMessageHandler(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<HttpLoggingHandler>>();
                return new HttpLoggingHandler(logger, request.ServiceType);
            });
        }
    }

    public class HttpLoggingHandler : DelegatingHandler
    {
        private readonly ILogger<HttpLoggingHandler> _logger;
        private readonly WeatherServiceType _serviceType;

        public HttpLoggingHandler(ILogger<HttpLoggingHandler> logger, WeatherServiceType serviceType)
        {
            _logger = logger;
            _serviceType = serviceType;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
        {
            var stopwatch = Stopwatch.StartNew();
            var requestId = Guid.NewGuid().ToString("N")[..8];

            _logger.LogInformation(
                "[{RequestId}] {ServiceType} → {Method} {Uri}",
                requestId, _serviceType, request.Method, request.RequestUri);

            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                stopwatch.Stop();

                var logLevel = response.IsSuccessStatusCode ? LogLevel.Information : LogLevel.Warning;
                var status = response.IsSuccessStatusCode ? "✓" : "✗";

                _logger.Log(logLevel,
                    "[{RequestId}] {ServiceType} {Status} {StatusCode} in {ElapsedMs}ms",
                    requestId, _serviceType, status, response.StatusCode, stopwatch.ElapsedMilliseconds);

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex,
                    "[{RequestId}] {ServiceType} ✗ FAILED after {ElapsedMs}ms",
                    requestId, _serviceType, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}