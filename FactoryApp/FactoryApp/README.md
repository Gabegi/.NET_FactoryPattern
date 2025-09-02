# FactoryApp (Factory pattern with HttpClient + DI)

This project demonstrates a pragmatic "factory" approach to creating typed `HttpClient` instances with custom handlers (logging, caching, resilience) in a .NET 8 Web API. It contrasts with the other samples in this repo by focusing on a lightweight factory plus DI registrations instead of keyed DI or abstract factories.

## Quick start

- Build:

```bash
cd FactoryApp/FactoryApp
dotnet build
```

- Run:

```bash
dotnet run
```

- Swagger UI (development): `https://localhost:7001/swagger`
- Weather endpoint: `GET https://localhost:7001/api/weather?serviceType=TokyoDevUser`

Note: On some Windows setups you may need to run the terminal as Administrator if you hit an "Access is denied" when starting the app.

## What this sample shows

- A factory (`ClientFactory`) that returns an `HttpClient` by name
- Custom `DelegatingHandler`s for logging and caching
- Centralised DI registration via an extension method
- Resilience configuration using `Microsoft.Extensions.Http.Resilience`

## Project layout

- `Application/WeatherService`
  - `IWeatherService`, `GetCurrentWeatherService`, `WeatherRequest`
- `Domain`
  - `WeatherServiceTypes.cs` enum with attributes
  - `Entities/Weather.cs`
  - `Attributes/FeaturesAttribute.cs`, `Attributes/ServiceConfigAttribute.cs`
- `Infrastructure`
  - `Factory/ClientFactory.cs` (returns named `HttpClient`)
  - `Handlers/LoggingHandler.cs`, `Handlers/CachingHandler.cs`, `Handlers/BaseClientHandler.cs`
  - `Interfaces/` (`IClientFactory`, `IBaseClientHandler`, `IWeatherClient`)
  - `OpenMeteoClient.cs` (calls the external API)
  - `Extensions/InfrastructureExtension.cs` (DI wiring for clients/handlers/resilience)
- `Presentation`
  - `Controllers/WeatherController.cs` (exposes API)
  - `DTOs/WeatherResponseDTO.cs`
- `Program.cs` (service registration / pipeline)

## Dependency Injection wiring

All infrastructure services and typed `HttpClient`s are registered in `Infrastructure/Extensions/InfrastructureExtension.cs` via `AddWeatherHttpClients()` and consumed in `Program.cs`:

- Registers base `HttpClient`, logging, `HybridCache`
- Adds named clients (e.g., `TokyoDevUser`, `NewYorkPrdAdmin`)
- Attaches handlers:
  - `LoggingHandler` – request/response logging with timings
  - `CachingHandler` – GET response caching backed by `HybridCache`
- Applies resilience with `AddStandardResilienceHandler` (timeouts, retries, circuit breaker)
- Registers `IBaseClientHandler`, `IClientFactory`

Packages used (see `.csproj`):

- `Microsoft.Extensions.Caching.Hybrid`
- `Microsoft.Extensions.Http.Resilience`
- `Microsoft.Extensions.Resilience`
- `Swashbuckle.AspNetCore`

## Weather service selection

The controller accepts `WeatherServiceType` via query string. Available values (see `Domain/WeatherServiceTypes.cs`):

- `TokyoDevUser`
- `NewYorkPrdAdmin`

These enum values are decorated with attributes:

- `ServiceConfigAttribute` provides base URL, region, environment, timezone, timeout, latitude/longitude
- `FeaturesAttribute` toggles features like retry, caching, logging

`BaseClientHandler` uses these attributes to set the base address and timeout for the created `HttpClient`.

## API usage

- GET `api/weather?serviceType=TokyoDevUser`
  - Returns current weather data mapped to `WeatherResponseDTO`
- Swagger UI shows the endpoint and schema in development.

Example curl:

```bash
curl "https://localhost:7001/api/weather?serviceType=TokyoDevUser" -k
```

## Extending the sample

1. Add a new service type

- Add a new value to `Domain/WeatherServiceTypes.cs` and decorate it with `ServiceConfig` and `Features` attributes

2. Register a named client

- In `AddWeatherHttpClients()` add a new `AddHttpClient("<Name>")` for the new service
- Attach handlers (logging, caching) and, if needed, tweak resilience

3. Call the service

- Pass the new enum value as `serviceType` to `GET /api/weather`
