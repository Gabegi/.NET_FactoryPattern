using FactoryApp.Infrastructure.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FactoryApp.Infrastructure.Factories;

// ✅ WITH FACTORY PATTERN - Complex Logic Centralized and Organized
public class WeatherServiceCreationRequest
{
    public string Environment { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public bool RequiresAuthentication { get; set; }
    public bool EnableCaching { get; set; } = true;
    public bool EnableRetryPolicy { get; set; } = true;
    public int? CustomTimeoutSeconds { get; set; }
    public string? CustomUserAgent { get; set; }
    public List<string> RequiredFeatures { get; set; } = new();
}

public class WeatherServiceFactory : IWeatherServiceFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;
    private readonly ILogger<WeatherServiceFactory> _logger;
    private readonly ICacheService _cacheService;
    private readonly IRetryPolicyService _retryService;

    public WeatherServiceFactory(
        IServiceProvider serviceProvider,
        IConfiguration configuration,
        ILogger<WeatherServiceFactory> logger,
        ICacheService cacheService,
        IRetryPolicyService retryService)
    {
        _serviceProvider = serviceProvider;
        _configuration = configuration;
        _logger = logger;
        _cacheService = cacheService;
        _retryService = retryService;
    }

    // ✅ CENTRALISED COMPLEX CREATION LOGIC
    public IWeatherService CreateWeatherService(WeatherServiceCreationRequest request)
    {
        _logger.LogInformation("Creating weather service for Environment: {Environment}, Region: {Region}", 
            request.Environment, request.Region);

        // Step 1: Determine base service type based on environment and region
        var baseService = CreateBaseService(request);

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

    // STEP 1: Complex conditional base service creation
    private IWeatherService CreateBaseService(WeatherServiceCreationRequest request)
    {
        // Mock service for development/testing
        if (IsNonProductionEnvironment(request.Environment))
        {
            _logger.LogDebug("Using mock weather service for non-production environment");
            return _serviceProvider.GetRequiredService<MockWeatherService>();
        }

        // Production services - complex regional logic
        return request.Region.ToLowerInvariant() switch
        {
            "europe" or "eu" => CreateEuropeanService(request),
            "northamerica" or "na" => CreateNorthAmericanService(request),
            "asia" => CreateAsianService(request),
            "global" => CreateGlobalService(request),
            _ => CreateDefaultService(request)
        };
    }

    private bool IsNonProductionEnvironment(string environment)
    {
        return environment.ToLowerInvariant() switch
        {
            "development" or "dev" or "test" or "staging" => true,
            _ => false
        };
    }

    private IWeatherService CreateEuropeanService(WeatherServiceCreationRequest request)
    {
        _logger.LogDebug("Creating European weather service");
        return _serviceProvider.GetRequiredService<OpenMeteoService>();
    }

    private IWeatherService CreateNorthAmericanService(WeatherServiceCreationRequest request)
    {
        _logger.LogDebug("Creating North American weather service");
        return _serviceProvider.GetRequiredService<OpenMeteoService>();
    }

    private IWeatherService CreateAsianService(WeatherServiceCreationRequest request)
    {
        _logger.LogDebug("Creating Asian weather service");
        return _serviceProvider.GetRequiredService<OpenMeteoService>();
    }

    private IWeatherService CreateGlobalService(WeatherServiceCreationRequest request)
    {
        _logger.LogDebug("Creating global weather service");
        return _serviceProvider.GetRequiredService<OpenMeteoService>();
    }

    private IWeatherService CreateDefaultService(WeatherServiceCreationRequest request)
    {
        _logger.LogDebug("Creating default weather service");
        return _serviceProvider.GetRequiredService<OpenMeteoService>();
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