# 🌤️ Abstract Factory App - Weather Service Factory Pattern

This application demonstrates the **Factory Pattern** with **Abstract Factory** concepts, providing a sophisticated weather service creation system.

## 🏗️ Architecture Overview

The AbstractFactoryApp implements the same complex factory pattern as the FactoryApp, but serves as a reference implementation for the Abstract Factory pattern. It showcases:

- **Factory Pattern**: Centralized service creation logic
- **Decorator Pattern**: Flexible service composition
- **Dependency Injection**: Clean service registration
- **Layered Architecture**: Clear separation of concerns

## 🚀 Features

### ✅ **Complex Factory Logic**
- Environment-aware service selection
- Regional service routing
- Conditional decorator application
- Feature validation
- Comprehensive logging

### ✅ **Service Decorators**
- **Caching**: In-memory cache with TTL
- **Retry Logic**: Exponential backoff retry
- **Authentication**: Authentication headers (placeholder)

### ✅ **Base Services**
- **MockWeatherService**: Development/testing
- **OpenMeteoService**: Production API integration

## 📁 Project Structure

```
AbstractFactoryApp/
├── Domain/
│   └── Entities/
│       └── Weather.cs                    # Weather data models
├── Application/
│   └── UseCases/
│       └── GetCurrentWeatherUseCase.cs   # Business logic
├── Infrastructure/
│   ├── Factories/
│   │   ├── IWeatherServiceFactory.cs     # Factory interface
│   │   └── WeatherServiceFactory.cs      # Main factory implementation
│   ├── Interfaces/
│   │   ├── IWeatherService.cs            # Base service interface
│   │   ├── ICacheService.cs              # Cache interface
│   │   ├── IRetryPolicyService.cs        # Retry interface
│   │   └── IConfigurableWeatherService.cs # Config interface
│   ├── Services/
│   │   ├── CachedWeatherService.cs       # Caching decorator
│   │   ├── RetryWeatherService.cs        # Retry decorator
│   │   ├── AuthenticatedWeatherService.cs # Auth decorator
│   │   ├── MemoryCacheService.cs         # Cache implementation
│   │   └── SimpleRetryPolicyService.cs   # Retry implementation
│   ├── MockWeatherService.cs             # Mock service
│   └── OpenMeteoService.cs               # Real API service
├── Presentation/
│   ├── Controllers/
│   │   └── WeatherController.cs          # API endpoints
│   └── DTOs/
│       └── WeatherResponseDTO.cs         # Response models
└── Program.cs                            # Dependency injection setup
```

## 🔧 Factory Pattern Implementation

### **4-Step Creation Process**

1. **CreateBaseService()**: Chooses base service based on environment/region
2. **ApplyConditionalDecorators()**: Wraps with caching, retry, authentication
3. **ConfigureServiceSettings()**: Sets custom timeouts and user agents
4. **ValidateServiceRequirements()**: Ensures feature support

### **Service Creation Request**

```csharp
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
```

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

### Development Environment
```http
POST /api/weather/advanced?lat=51.5&lon=-0.1
Content-Type: application/json

{
  "environment": "development",
  "region": "global",
  "enableCaching": false,
  "enableRetryPolicy": false,
  "requiredFeatures": ["current_weather"]
}
```

## 🎯 Benefits

✅ **Separation of Concerns**: Each component has a single responsibility  
✅ **Extensibility**: Easy to add new services and decorators  
✅ **Testability**: Services can be easily mocked and tested  
✅ **Flexibility**: Dynamic service creation based on requirements  
✅ **Maintainability**: Clear structure and dependencies  
✅ **Scalability**: Can handle complex service creation scenarios  

## 🔄 Request Flow

1. **Client Request** → WeatherController
2. **Use Case Processing** → Business logic and validation
3. **Factory Creation** → Complex service creation logic
4. **Service Execution** → Decorated service chain execution
5. **Response Return** → Formatted weather data

## 🛠️ Running the Application

1. **Build the project**:
   ```bash
   dotnet build
   ```

2. **Run the application**:
   ```bash
   dotnet run
   ```

3. **Access the API**:
   - Swagger UI: `https://localhost:7002/swagger`
   - Basic endpoint: `GET https://localhost:7002/api/weather`
   - Advanced endpoint: `POST https://localhost:7002/api/weather/advanced`

4. **Test with HTTP file**:
   Use the provided `AbstractFactoryApp.http` file for testing

## 📊 Comparison with FactoryApp

This AbstractFactoryApp implements the **exact same factory pattern** as the FactoryApp, serving as:

- **Reference Implementation**: Clean, well-structured factory pattern
- **Learning Resource**: Demonstrates best practices
- **Comparison Base**: Shows how the same pattern can be applied consistently
- **Abstract Factory Example**: Illustrates abstract factory concepts

Both applications provide identical functionality but serve different educational purposes in understanding factory patterns.
