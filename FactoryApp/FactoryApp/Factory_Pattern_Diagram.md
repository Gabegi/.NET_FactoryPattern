# 🌤️ Weather Service Factory Pattern Architecture

## 📋 System Overview

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              WEATHER SERVICE FACTORY PATTERN                    │
│                              ================================================    │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## 🏗️ Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                 PRESENTATION LAYER                              │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                    WeatherController                                    │   │
│  │  ┌─────────────────┐  ┌─────────────────┐                              │   │
│  │  │   GET /api/     │  │ POST /api/      │                              │   │
│  │  │   weather       │  │ weather/        │                              │   │
│  │  │                 │  │ advanced        │                              │   │
│  │  └─────────────────┘  └─────────────────┘                              │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                    │                                           │
│                                    ▼                                           │
└─────────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                                APPLICATION LAYER                                │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                GetCurrentWeatherUseCase                                 │   │
│  │                                                                         │   │
│  │  ┌─────────────────────────────────────────────────────────────────┐   │   │
│  │  │  ExecuteAsync(lat, lon, serviceType)                           │   │   │
│  │  │  └─► Creates WeatherServiceCreationRequest                     │   │   │
│  │  │                                                                 │   │   │
│  │  │  ExecuteAsync(lat, lon, WeatherServiceCreationRequest)         │   │   │
│  │  │  └─► Passes request to factory                                  │   │   │
│  │  └─────────────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                    │                                           │
│                                    ▼                                           │
└─────────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              INFRASTRUCTURE LAYER                               │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                    FACTORY PATTERN CORE                                 │   │
│  │  ┌─────────────────────────────────────────────────────────────────┐   │   │
│  │  │              IWeatherServiceFactory                             │   │   │
│  │  │  ┌─────────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  CreateWeatherService(WeatherServiceCreationRequest)    │   │   │   │
│  │  │  └─────────────────────────────────────────────────────────┘   │   │   │
│  │  └─────────────────────────────────────────────────────────────────┘   │   │
│  │                                    │                                     │   │
│  │                                    ▼                                     │   │
│  │  ┌─────────────────────────────────────────────────────────────────┐   │   │
│  │  │              WeatherServiceFactory                              │   │   │
│  │  │                                                                 │   │   │
│  │  │  ┌─────────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  STEP 1: CreateBaseService()                            │   │   │   │
│  │  │  │  ├─► IsNonProductionEnvironment()                      │   │   │   │
│  │  │  │  ├─► CreateEuropeanService()                           │   │   │   │
│  │  │  │  ├─► CreateNorthAmericanService()                      │   │   │   │
│  │  │  │  ├─► CreateAsianService()                              │   │   │   │
│  │  │  │  └─► CreateGlobalService()                             │   │   │   │
│  │  │  └─────────────────────────────────────────────────────────┘   │   │   │
│  │  │                                    │                             │   │   │
│  │  │                                    ▼                             │   │   │
│  │  │  ┌─────────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  STEP 2: ApplyConditionalDecorators()                  │   │   │   │
│  │  │  │  ├─► CachedWeatherService (if EnableCaching)           │   │   │   │
│  │  │  │  ├─► RetryWeatherService (if EnableRetryPolicy)        │   │   │   │
│  │  │  │  └─► AuthenticatedWeatherService (if RequiresAuth)     │   │   │   │
│  │  │  └─────────────────────────────────────────────────────────┘   │   │   │
│  │  │                                    │                             │   │   │
│  │  │                                    ▼                             │   │   │
│  │  │  ┌─────────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  STEP 3: ConfigureServiceSettings()                     │   │   │   │
│  │  │  │  ├─► SetTimeout() (if CustomTimeoutSeconds)             │   │   │   │
│  │  │  │  └─► SetUserAgent() (if CustomUserAgent)                │   │   │   │
│  │  │  └─────────────────────────────────────────────────────────┘   │   │   │
│  │  │                                    │                             │   │   │
│  │  │                                    ▼                             │   │   │
│  │  │  ┌─────────────────────────────────────────────────────────┐   │   │   │
│  │  │  │  STEP 4: ValidateServiceRequirements()                  │   │   │   │
│  │  │  │  └─► SupportsFeature() for each RequiredFeatures       │   │   │   │
│  │  │  └─────────────────────────────────────────────────────────┘   │   │   │
│  │  └─────────────────────────────────────────────────────────────────┘   │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                    │                                           │
│                                    ▼                                           │
└─────────────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              SERVICE IMPLEMENTATIONS                            │
├─────────────────────────────────────────────────────────────────────────────────┤
│                                                                                 │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                        BASE SERVICES                                     │   │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐        │   │
│  │  │ MockWeather     │  │ OpenMeteo       │  │ (Future:        │        │   │
│  │  │ Service         │  │ Service         │  │  Other APIs)    │        │   │
│  │  │                 │  │                 │  │                 │        │   │
│  │  │ • Development   │  │ • Production    │  │ • Regional      │        │   │
│  │  │ • Testing       │  │ • Real API      │  │ • Specialized   │        │   │
│  │  │ • Mock Data     │  │ • Open-Meteo    │  │ • Enterprise    │        │   │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘        │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                    │                                           │
│                                    ▼                                           │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                      DECORATOR SERVICES                                  │   │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐        │   │
│  │  │ CachedWeather   │  │ RetryWeather    │  │ Authenticated   │        │   │
│  │  │ Service         │  │ Service         │  │ WeatherService  │        │   │
│  │  │                 │  │                 │  │                 │        │   │
│  │  │ • In-Memory     │  │ • Exponential   │  │ • Auth Headers  │        │   │
│  │  │   Cache         │  │   Backoff       │  │ • API Keys      │        │   │
│  │  │ • TTL Support   │  │ • Retry Logic   │  │ • OAuth         │        │   │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘        │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
│                                    │                                           │
│                                    ▼                                           │
│  ┌─────────────────────────────────────────────────────────────────────────┐   │
│  │                      SUPPORTING SERVICES                                 │   │
│  │  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────┐        │   │
│  │  │ MemoryCache     │  │ SimpleRetry     │  │ IConfigurable   │        │   │
│  │  │ Service         │  │ PolicyService   │  │ WeatherService  │        │   │
│  │  │                 │  │                 │  │                 │        │   │
│  │  │ • Dictionary    │  │ • Exponential   │  │ • SetTimeout()  │        │   │
│  │  │ • Expiration    │  │   Backoff       │  │ • SetUserAgent()│        │   │
│  │  │ • Thread-Safe   │  │ • Logging       │  │ • Configuration │        │   │
│  │  └─────────────────┘  └─────────────────┘  └─────────────────┘        │   │
│  └─────────────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────────────┘
```

## 🔄 Request Flow Diagram

```
┌─────────────────────────────────────────────────────────────────────────────────┐
│                              REQUEST FLOW                                      │
└─────────────────────────────────────────────────────────────────────────────────┘

1. CLIENT REQUEST
   ┌─────────────────┐
   │ WeatherController│
   │ GET /api/weather │
   └─────────┬───────┘
             │
             ▼

2. USE CASE PROCESSING
   ┌─────────────────────────┐
   │ GetCurrentWeatherUseCase│
   │ • Validate coordinates  │
   │ • Create request object │
   └─────────┬───────────────┘
             │
             ▼

3. FACTORY CREATION
   ┌─────────────────────────┐
   │ WeatherServiceFactory   │
   │ • Analyze request       │
   │ • Choose base service   │
   │ • Apply decorators      │
   │ • Configure settings    │
   │ • Validate features     │
   └─────────┬───────────────┘
             │
             ▼

4. SERVICE EXECUTION
   ┌─────────────────────────┐
   │ Decorated Weather       │
   │ Service Chain:          │
   │ Cached → Retry → Auth → │
   │ Base Service            │
   └─────────┬───────────────┘
             │
             ▼

5. RESPONSE RETURN
   ┌─────────────────────────┐
   │ Weather Data            │
   │ • Current weather       │
   │ • Formatted response    │
   └─────────────────────────┘
```

## 📁 File Structure & Dependencies

```
FactoryApp/
├── Presentation/
│   └── Controllers/
│       └── WeatherController.cs          # Entry point
│           ├── GET /api/weather          # Basic endpoint
│           └── POST /api/weather/advanced # Advanced factory endpoint
│
├── Application/
│   └── UseCases/
│       └── GetCurrentWeatherUseCase.cs   # Business logic
│           ├── ExecuteAsync(lat, lon, serviceType)
│           └── ExecuteAsync(lat, lon, request)
│
├── Infrastructure/
│   ├── Factories/
│   │   ├── IWeatherServiceFactory.cs     # Factory interface
│   │   └── WeatherServiceFactory.cs      # Main factory implementation
│   │       ├── CreateBaseService()
│   │       ├── ApplyConditionalDecorators()
│   │       ├── ConfigureServiceSettings()
│   │       └── ValidateServiceRequirements()
│   │
│   ├── Interfaces/
│   │   ├── IWeatherService.cs            # Base service interface
│   │   ├── ICacheService.cs              # Cache interface
│   │   ├── IRetryPolicyService.cs        # Retry interface
│   │   └── IConfigurableWeatherService.cs # Config interface
│   │
│   ├── Services/
│   │   ├── CachedWeatherService.cs       # Caching decorator
│   │   ├── RetryWeatherService.cs        # Retry decorator
│   │   ├── AuthenticatedWeatherService.cs # Auth decorator
│   │   ├── MemoryCacheService.cs         # Cache implementation
│   │   └── SimpleRetryPolicyService.cs   # Retry implementation
│   │
│   ├── MockWeatherService.cs             # Mock service
│   └── OpenMeteoService.cs               # Real API service
│
└── Program.cs                             # Dependency injection setup
```

## 🔧 Key Components & Responsibilities

### **Factory Pattern Core**

- **`WeatherServiceCreationRequest`**: Configuration object for service creation
- **`IWeatherServiceFactory`**: Interface defining factory contract
- **`WeatherServiceFactory`**: Main factory with 4-step creation process

### **Base Services**

- **`MockWeatherService`**: Development/testing service with mock data
- **`OpenMeteoService`**: Production service using real API

### **Decorator Pattern**

- **`CachedWeatherService`**: Adds in-memory caching
- **`RetryWeatherService`**: Adds retry logic with exponential backoff
- **`AuthenticatedWeatherService`**: Adds authentication (placeholder)

### **Supporting Services**

- **`MemoryCacheService`**: In-memory cache implementation
- **`SimpleRetryPolicyService`**: Retry logic implementation
- **`IConfigurableWeatherService`**: Interface for configurable services

## 🎯 Benefits of This Architecture

✅ **Separation of Concerns**: Each component has a single responsibility  
✅ **Extensibility**: Easy to add new services and decorators  
✅ **Testability**: Services can be easily mocked and tested  
✅ **Flexibility**: Dynamic service creation based on requirements  
✅ **Maintainability**: Clear structure and dependencies  
✅ **Scalability**: Can handle complex service creation scenarios

## 🚀 Usage Examples

### Basic Usage (Backward Compatible)

```http
GET /api/weather?lat=51.5&lon=-0.1&service=openmeteo
```

### Advanced Factory Usage

```http
POST /api/weather/advanced?lat=51.5&lon=-0.1
Content-Type: application/json

{
  "environment": "production",
  "region": "europe",
  "enableCaching": true,
  "enableRetryPolicy": true,
  "requiredFeatures": ["current_weather", "forecast"]
}
```

This architecture provides a robust, flexible, and maintainable solution for creating weather services with complex requirements!
