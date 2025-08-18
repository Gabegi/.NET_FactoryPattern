using FactoryApp.Domain.Entities;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Infrastructure.Services;

namespace FactoryApp.Infrastructure.Factories;

public class WeatherClientFactory : IWeatherClientFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherClientFactory> _logger;
    private readonly ICacheService _cacheService;
    private readonly IRetryPolicyService _retryService;
    private readonly IBaseWeatherService _baseService;

    public WeatherClientFactory(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<WeatherClientFactory> logger,
        ICacheService cacheService,
        IRetryPolicyService retryService,
        IBaseWeatherService baseService)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
        _cacheService = cacheService;
        _retryService = retryService;
        _baseService = baseService;
    }

    // ✅ steps
    // create service based on env and region
    // Apply conditional decorators based on requirements

    public IWeatherClient CreateClient(WeatherClientCreationRequest request)
    {
        if (!Enum.TryParse<WeatherServiceType>(serviceName, out var serviceType))
        {
            throw new ArgumentException($"Invalid service name: {serviceName}. Must be one of: {string.Join(", ", Enum.GetNames<WeatherServiceType>())}");
        }

        var httpClient = _httpClientFactory.CreateClient("WeatherService");
        return new UnifiedWeatherService(httpClient, serviceType, _logger);
    }



    // STEP 2: Apply conditional decorators
    private IWeatherClient ApplyConditionalDecorators(IWeatherClient baseService, WeatherClientCreationRequest request)
    {
        var service = baseService;

        if (request.EnableCaching)
        {
            service = new CachedWeatherService(service, _cacheService);
            _logger.LogDebug("Applied caching decorator");
        }

        if (request.EnableRetryPolicy)
        {
            service = new RetryWeatherService(service, _retryService);
            _logger.LogDebug("Applied retry policy decorator");
        }

        if (request.RequiresAuthentication)
        {
            service = new AuthenticatedWeatherService(service, _configuration);
            _logger.LogDebug("Applied authentication decorator");
        }

        return service;
    }

    // STEP 3: Configure service settings
    private void ConfigureServiceSettings(IWeatherClient service, WeatherClientCreationRequest request)
    {
        if (request.CustomTimeoutSeconds.HasValue)
        {
            // Configure timeout if service supports it
            if (service is IConfigurableWeatherService configurableService)
            {
                configurableService.SetTimeout(request.CustomTimeoutSeconds.Value);
                _logger.LogDebug("Configured custom timeout: {Timeout}s", request.CustomTimeoutSeconds.Value);
            }
        }

        if (!string.IsNullOrEmpty(request.CustomUserAgent))
        {
            // Configure user agent if service supports it
            if (service is IConfigurableWeatherService configurableService)
            {
                configurableService.SetUserAgent(request.CustomUserAgent);
                _logger.LogDebug("Configured custom user agent: {UserAgent}", request.CustomUserAgent);
            }
        }
    }

    // STEP 4: Validate service requirements
    private void ValidateServiceRequirements(IWeatherClient service, WeatherClientCreationRequest request)
    {
        foreach (var feature in request.RequiredFeatures)
        {
            if (!service.SupportsFeature(feature))
            {
                throw new InvalidOperationException($"Weather service does not support required feature: {feature}");
            }
        }

        _logger.LogDebug("Service validation passed for {FeatureCount} features", request.RequiredFeatures.Count);
    }
} 