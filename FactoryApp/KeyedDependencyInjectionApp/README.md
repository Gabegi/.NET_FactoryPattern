# Keyed Dependency Injection App

This application demonstrates the use of **Keyed Dependency Injection** in .NET 8, allowing runtime selection of services based on keys.

## What is Keyed Dependency Injection?

Keyed dependency injection allows you to register multiple implementations of the same interface with different keys, and then select which implementation to use at runtime. This is particularly useful when you have multiple implementations of the same service and need to choose between them based on configuration, user preference, or other runtime conditions.

## Key Features

- **Runtime Service Selection**: Choose between different weather service implementations at runtime
- **Multiple Service Implementations**: Mock service for testing and OpenMeteo service for real data
- **Flexible API**: Multiple ways to specify which service to use
- **Error Handling**: Proper validation and error responses

## Service Implementations

### 1. MockWeatherService (`"mock"`)
- Returns simulated weather data
- Fast response time (100ms delay)
- No external dependencies
- Perfect for testing and development

### 2. OpenMeteoService (`"openmeteo"`)
- Fetches real weather data from Open-Meteo API
- Configurable timeout and user agent
- Requires internet connection
- Returns actual weather data

## API Endpoints

### Main Endpoint
```
GET /api/weather?lat={latitude}&lon={longitude}&service={serviceKey}
```

**Parameters:**
- `lat`: Latitude (default: 51.5)
- `lon`: Longitude (default: -0.1)
- `service`: Service key - either "mock" or "openmeteo" (default: "openmeteo")

### Specific Service Endpoints
```
GET /api/weather/mock?lat={latitude}&lon={longitude}
GET /api/weather/openmeteo?lat={latitude}&lon={longitude}
```

## Usage Examples

### Using Query Parameter
```bash
# Use mock service
curl "https://localhost:7001/api/weather?lat=51.5&lon=-0.1&service=mock"

# Use OpenMeteo service
curl "https://localhost:7001/api/weather?lat=51.5&lon=-0.1&service=openmeteo"
```

### Using Specific Endpoints
```bash
# Mock service endpoint
curl "https://localhost:7001/api/weather/mock?lat=51.5&lon=-0.1"

# OpenMeteo service endpoint
curl "https://localhost:7001/api/weather/openmeteo?lat=51.5&lon=-0.1"
```

## Configuration

The application uses `appsettings.json` to configure the OpenMeteo service:

```json
{
  "WeatherApi": {
    "BaseUrl": "https://api.open-meteo.com/v1",
    "TimeoutSeconds": 30,
    "UserAgent": "KeyedWeatherApp/1.0"
  }
}
```

## Keyed Service Registration

In `Program.cs`, services are registered with keys:

```csharp
// Keyed dependency injection - register services with keys
builder.Services.AddKeyedScoped<IWeatherService, MockWeatherService>("mock");
builder.Services.AddKeyedScoped<IWeatherService, OpenMeteoService>("openmeteo");
```

## Service Resolution

In the use case, the service is resolved by key:

```csharp
// Keyed dependency injection - get the service by key
var weatherService = _serviceProvider.GetRequiredKeyedService<IWeatherService>(serviceKey);
```

## Response Format

All endpoints return a JSON response with the following structure:

```json
{
  "service": "mock|openmeteo",
  "data": {
    "latitude": 51.5,
    "longitude": -0.1,
    "current_weather": {
      "temperature": 21.4,
      "windspeed": 5.5,
      "winddirection": 180,
      "is_day": 1,
      "weathercode": 2
    },
    // ... other weather data
  }
}
```

## Error Handling

- **Invalid Coordinates**: Returns 400 Bad Request with validation message
- **Invalid Service Key**: Returns 400 Bad Request with error message
- **Service Unavailable**: Returns 404 Not Found
- **Network Errors**: Handled gracefully with appropriate logging

## Benefits of Keyed Dependency Injection

1. **Runtime Flexibility**: Choose services based on user preferences or configuration
2. **Testing**: Easy to switch between real and mock services
3. **A/B Testing**: Can easily switch between different implementations
4. **Feature Flags**: Enable/disable features by switching service implementations
5. **Environment-Specific Behavior**: Different services for development, staging, and production

## Running the Application

1. Navigate to the project directory
2. Run `dotnet run`
3. The API will be available at `https://localhost:7001`
4. Use the provided HTTP file or curl commands to test the endpoints

## Testing

Use the included `KeyedDependencyInjectionApp.http` file to test all endpoints with different service selections. 