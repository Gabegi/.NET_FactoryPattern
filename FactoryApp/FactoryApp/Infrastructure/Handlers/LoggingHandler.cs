using FactoryApp.Domain;
using System.Diagnostics;

namespace FactoryApp.Infrastructure.Handlers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger<LoggingHandler> _logger;
        private readonly WeatherServiceType _serviceType;

        public LoggingHandler(ILogger<LoggingHandler> logger, WeatherServiceType serviceType)
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