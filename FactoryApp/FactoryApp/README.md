# FactoryApp - Factory Method Pattern Implementation

This application demonstrates the **Factory Method Pattern** in a .NET Web API context, following the same structure as the DependencyInjectionApp and KeyedDependencyInjectionApp but using the Factory Method pattern to create service objects.

## Architecture Overview

The application follows Clean Architecture principles with the following layers:

- **Domain**: Contains the Weather entity
- **Infrastructure**: Contains service implementations and the factory
- **Application**: Contains use cases
- **Presentation**: Contains controllers and DTOs

## Factory Method Pattern Implementation

### Key Components

1. **IWeatherServiceFactory** (`Infrastructure/Factories/IWeatherServiceFactory.cs`)
   - Factory interface that defines the contract for creating weather services

2. **WeatherServiceFactory** (`Infrastructure/Factories/WeatherServiceFactory.cs`)
   - Concrete factory implementation that creates different types of weather services based on a service type parameter

3. **Weather Services**
   - `MockWeatherService`: Returns mock weather data
   - `OpenMeteoService`: Fetches real weather data from Open-Meteo API

### How the Factory Method Works

The Factory Method pattern is implemented as follows:

1. The `IWeatherServiceFactory` interface defines a method to create weather services
2. The `WeatherServiceFactory` concrete implementation uses a service type parameter to determine which service to create
3. The `GetCurrentWeatherUseCase` uses the factory to get the appropriate service
4. The controller accepts a `service` query parameter to specify which service to use

## API Endpoints

### GET /api/weather

Retrieves current weather information.

**Query Parameters:**
- `lat` (optional): Latitude (default: 51.5)
- `lon` (optional): Longitude (default: -0.1)
- `service` (optional): Service type - "mock" or "openmeteo" (default: "openmeteo")

**Example Requests:**
```
GET /api/weather?lat=40.7128&lon=-74.0060&service=openmeteo
GET /api/weather?lat=40.7128&lon=-74.0060&service=mock
```

## Configuration

The application uses configuration from `appsettings.json`:

```json
{
  "WeatherApi": {
    "TimeoutSeconds": 30,
    "UserAgent": "FactoryApp/1.0"
  }
}
```

## Running the Application

1. Navigate to the FactoryApp directory
2. Run the application:
   ```bash
   dotnet run
   ```
3. Access the API at `https://localhost:7001/api/weather`
4. Access Swagger UI at `https://localhost:7001/swagger`

## Factory Method Pattern Benefits

1. **Flexibility**: Easy to switch between different service implementations at runtime
2. **Extensibility**: New service types can be added by implementing the interface and updating the factory
3. **Separation of Concerns**: Service creation logic is centralized in the factory
4. **Testability**: Easy to mock the factory for testing different scenarios

## Comparison with Other Patterns

- **DependencyInjectionApp**: Uses direct dependency injection with multiple service registrations
- **KeyedDependencyInjectionApp**: Uses .NET 8's keyed services for service selection
- **FactoryApp**: Uses the Factory Method pattern for service creation and selection

## Adding New Weather Services

To add a new weather service:

1. Implement `IWeatherService` interface
2. Register the service in `Program.cs`
3. Update `WeatherServiceFactory.CreateWeatherService()` method to handle the new service type 