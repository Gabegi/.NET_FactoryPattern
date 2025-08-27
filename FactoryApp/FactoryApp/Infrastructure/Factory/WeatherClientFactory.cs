//using FactoryApp.Domain.Entities;
//using FactoryApp.Infrastructure.Interfaces;
//using System.Reflection;

//namespace FactoryApp.Infrastructure.Factories;

//public class WeatherClientFactory : IWeatherClientFactory
//{
//    private readonly IConfiguration _configuration;
//    private readonly ILogger<WeatherClientFactory> _logger;
//    private readonly IBaseClient _baseClient;

//    public WeatherClientFactory(
//        IConfiguration configuration,
//        ILogger<WeatherClientFactory> logger,
//        IBaseClient baseService)
//    {
//        _configuration = configuration;
//        _logger = logger;
//        _baseClient = baseService;
//    }

//    public IWeatherClient CreateClient(WeatherClientCreationRequest request)
//    {
//        _logger.LogDebug("Creating weather client for service: {ServiceName}", request.ServiceName);

//        var clientAttributes = GetClientAttributes(request);

//        // Create base http weather client
//        var baseClient = _baseClient.CreateBaseClient(request, clientAttributes);

//        // STEP 2: Apply conditional decorators
//        var httpClient = CustomiseHttpClient(baseClient, request);

//        // STEP 3: Configure service settings
//        ConfigureServiceSettings(httpClient, request);

//        // STEP 4: Validate service requirements
//        ValidateServiceRequirements(httpClient, request);

//        _logger.LogInformation("Successfully created weather client for service: {ServiceName}", request.ServiceName);
//        return httpClient;
//    }

//    private static WeatherServiceConfigAttribute GetClientAttributes(WeatherClientCreationRequest request)
//    {
//        if (!Enum.TryParse<WeatherServiceType>(request.ServiceName, out var serviceType))
//        {
//            throw new ArgumentException($"Invalid service name: {request.ServiceName}");
//        }

//        var memberInfo = typeof(WeatherServiceType).GetMember(serviceType.ToString()).FirstOrDefault();
//        var attribute = memberInfo?.GetCustomAttribute<WeatherServiceConfigAttribute>();
//        if (attribute == null)
//        { throw new InvalidOperationException($"No config found for {serviceType}"); }
//        return attribute;
//    }



//    // STEP 2: Apply conditional decorators
//    private HttpClient CustomiseHttpClient(HttpClient client, WeatherClientCreationRequest request)
//    {


//        if (request.EnableCaching)
//        {
//            client.
//            _logger.LogDebug("Applied caching decorator");
//        }

//        if (request.EnableRetryPolicy)
//        {
//            service = new RetryWeatherService(service, _retryService);
//            _logger.LogDebug("Applied retry policy decorator");
//        }

//        if (request.RequiresAuthentication)
//        {
//            service = new AuthenticatedWeatherService(service, _configuration);
//            _logger.LogDebug("Applied authentication decorator");
//        }

//        return service;
//    }

//    // STEP 3: Configure service settings
//    private void ConfigureServiceSettings(IWeatherClient service, WeatherClientCreationRequest request)
//    {
//        if (request.CustomTimeoutSeconds.HasValue)
//        {
//            // Configure timeout if service supports it
//            if (service is IConfigurableWeatherService configurableService)
//            {
//                configurableService.SetTimeout(request.CustomTimeoutSeconds.Value);
//                _logger.LogDebug("Configured custom timeout: {Timeout}s", request.CustomTimeoutSeconds.Value);
//            }
//        }

//        if (!string.IsNullOrEmpty(request.CustomUserAgent))
//        {
//            // Configure user agent if service supports it
//            if (service is IConfigurableWeatherService configurableService)
//            {
//                configurableService.SetUserAgent(request.CustomUserAgent);
//                _logger.LogDebug("Configured custom user agent: {UserAgent}", request.CustomUserAgent);
//            }
//        }
//    }

//    // STEP 4: Validate service requirements
//    private void ValidateServiceRequirements(IWeatherClient service, WeatherClientCreationRequest request)
//    {
//        foreach (var feature in request.RequiredFeatures)
//        {
//            if (!service.SupportsFeature(feature))
//            {
//                throw new InvalidOperationException($"Weather service does not support required feature: {feature}");
//            }
//        }

//        _logger.LogDebug("Service validation passed for {FeatureCount} features", request.RequiredFeatures.Count);
//    }
//} 