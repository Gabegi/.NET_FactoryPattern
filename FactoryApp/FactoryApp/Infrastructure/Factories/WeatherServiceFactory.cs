using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Infrastructure.Services;

namespace FactoryApp.Infrastructure.Factories;

public class WeatherServiceFactory : IWeatherServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherServiceFactory> _logger;
    private readonly ICacheService _cacheService;
    private readonly IRetryPolicyService _retryService;
    private readonly IBaseWeatherService _baseService;

    public WeatherServiceFactory(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<WeatherServiceFactory> logger,
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

    // ✅ CENTRALISED COMPLEX CREATION LOGIC
    public IWeatherService CreateWeatherService(string serviceName)
    {
        _logger.LogInformation($"Creating weather service for service: {serviceName}");

        // Step 1: Determine base service type based on environment and region
        var baseService = _baseService.CreateBaseService(serviceName);

        // Step 2: Apply conditional decorators based on requirements
        var decoratedService = ApplyConditionalDecorators(baseService, request);

        // Step 3: Configure service with environment-specific settings
        ConfigureServiceSettings(decoratedService, request);

        // Step 4: Validate service meets all requirements
        ValidateServiceRequirements(decoratedService, request);

        _logger.LogInformation("Successfully created weather service: {ServiceType}", 
            decoratedService.GetType().Name);

        return decoratedService;
    }

   

    // STEP 2: Apply conditional decorators
    private IWeatherService ApplyConditionalDecorators(IWeatherService baseService, WeatherServiceCreationRequest request)
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
    private void ConfigureServiceSettings(IWeatherService service, WeatherServiceCreationRequest request)
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
    private void ValidateServiceRequirements(IWeatherService service, WeatherServiceCreationRequest request)
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