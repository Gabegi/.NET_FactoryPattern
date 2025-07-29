# Dependency Injection Weather App (Clean Architecture)

This project demonstrates the same weather API functionality as the FactoryApp, but implemented using **Dependency Injection** and **Clean Architecture** principles.

## Clean Architecture Structure

```
DependencyInjectionApp/
├── Domain/                    # Core business logic
│   ├── Entities/             # Domain entities (Weather)
│   └── Interfaces/           # Repository interfaces
├── Application/              # Application business rules
│   └── UseCases/            # Use cases (GetCurrentWeatherUseCase)
├── Infrastructure/           # External concerns
│   ├── Services/            # Repository implementations
│   └── DTOs/               # Data transfer objects for external APIs
└── Presentation/            # User interface
    └── Controllers/         # API controllers
```

## Key Differences from Factory Pattern

### Factory Pattern (FactoryApp)

- Uses a static `WeatherServiceFactory` class
- Manually creates dependencies (`HttpClient`)
- Direct instantiation in the controller
- Harder to test and mock
- No clear separation of concerns

### Clean Architecture with DI (This App)

- **Domain Layer**: Core business entities and interfaces
- **Application Layer**: Use cases and business rules
- **Infrastructure Layer**: External services and data access
- **Presentation Layer**: Controllers and API endpoints
- Services are registered in the DI container
- Dependencies are automatically injected
- Easy to test and mock
- Clear separation of concerns

## Implementation Details

The application follows Clean Architecture principles with clear separation between layers:

- **Domain Layer**: Contains the Weather entity and IWeatherRepository interface
- **Application Layer**: Contains use cases that implement business rules and validation
- **Infrastructure Layer**: Contains repository implementations and DTOs for external APIs
- **Presentation Layer**: Contains controllers that handle HTTP requests

Services are registered in the DI container in Program.cs, making it easy to swap implementations (e.g., real vs mock repositories).

## Benefits of Clean Architecture with DI

1. **Separation of Concerns**: Each layer has a specific responsibility
2. **Testability**: Easy to mock dependencies for unit testing
3. **Flexibility**: Easy to swap implementations
4. **Maintainability**: Clear structure makes code easier to understand
5. **Domain-Driven Design**: Business logic is isolated in the domain layer
6. **Dependency Inversion**: High-level modules don't depend on low-level modules

## Testing

With Clean Architecture and DI, testing becomes much easier as you can easily mock dependencies and test each layer independently.

## Running the Application

```bash
dotnet run
```

The API will be available at:

- Swagger UI: https://localhost:7001/swagger
- Weather API: https://localhost:7001/api/weather

## API Endpoints

- `GET /api/weather?lat=51.5&lon=-0.1` - Get current weather for specified coordinates

The response includes domain logic with properties like temperature, wind speed, units, timestamp, and weather conditions (isCold, isWarm, isWindy).
