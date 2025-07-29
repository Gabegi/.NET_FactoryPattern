# Dependency Injection Weather App

This project demonstrates the same weather API functionality as the FactoryApp, but implemented using **Dependency Injection** instead of the Factory Pattern.

## Key Differences from Factory Pattern

### Factory Pattern (FactoryApp)

- Uses a static `WeatherServiceFactory` class
- Manually creates dependencies (`HttpClient`)
- Direct instantiation in the controller
- Harder to test and mock

### Dependency Injection (This App)

- Services are registered in the DI container
- Dependencies are automatically injected
- Easier to test and mock
- Better separation of concerns
- More flexible and maintainable

## Implementation Details

### Service Registration

```csharp
// In Program.cs
builder.Services.AddHttpClient();
builder.Services.AddScoped<IWeatherService, OpenMeteoService>();

// To use mock service instead:
// builder.Services.AddScoped<IWeatherService, MockWeatherService>();
```

### Controller Usage

```csharp
public class WeatherController : ControllerBase
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService) // Injected by DI
    {
        _weatherService = weatherService;
    }
}
```

## Benefits of Dependency Injection

1. **Testability**: Easy to mock services for unit testing
2. **Flexibility**: Easy to swap implementations
3. **Lifetime Management**: DI container manages object lifetimes
4. **Loose Coupling**: Components don't create their own dependencies
5. **Configuration**: Centralized service configuration

## Testing Example

With DI, testing becomes much easier:

```csharp
[Test]
public async Task WeatherController_Get_ReturnsWeatherData()
{
    // Arrange
    var mockWeatherService = new Mock<IWeatherService>();
    mockWeatherService.Setup(x => x.GetCurrentWeatherAsync(51.5, -0.1))
        .ReturnsAsync(new WeatherInfo { Temperature = 22.5, WindSpeed = 5.2 });

    var controller = new WeatherController(mockWeatherService.Object);

    // Act
    var result = await controller.Get(51.5, -0.1);

    // Assert
    Assert.IsInstanceOf<OkObjectResult>(result);
}
```

## Running the Application

```bash
dotnet run
```

The API will be available at:

- Swagger UI: https://localhost:7001/swagger
- Weather API: https://localhost:7001/api/weather

## API Endpoints

- `GET /api/weather?lat=51.5&lon=-0.1` - Get current weather for specified coordinates
