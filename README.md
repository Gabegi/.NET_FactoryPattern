# .NET Factory & DI Patterns (Solution Overview)

This solution demonstrates several ways to compose services in .NET 8 Web APIs:

- Classic dependency injection (DI)
- Keyed dependency injection (keyed DI)
- Factory-based composition (lightweight factory + DI)
- Abstract factory (more formalized factory pattern)

Each project exposes a simple weather endpoint and shows different approaches to selecting/configuring the underlying implementation.

## Prerequisites

- .NET 8 SDK

## Projects

- `FactoryApp/DependencyInjectionApp`

  - Demonstrates classic DI: multiple implementations registered, chosen by app logic
  - Includes unit tests (`Tests/WeatherServiceUnitTests.cs`)

- `FactoryApp/KeyedDependencyInjectionApp`

  - Demonstrates .NET 8 keyed DI to choose implementations by key

- `FactoryApp/FactoryApp`

  - Demonstrates a lightweight factory that returns named `HttpClient` instances with custom handlers (logging, caching, resilience)
  - Uses a `ClientFactory` and `DelegatingHandler`s wired via DI

- `FactoryApp/AbstractFactoryApp`
  - Demonstrates an abstract factory approach to construct weather services with cross-cutting concerns (retry, caching, auth)

## Build the whole solution

```bash
cd FactoryApp
dotnet build FactoryApp.sln
```

## Run an individual project

You can run any sample independently. Below are common commands and endpoints.

- DependencyInjectionApp

```bash
cd FactoryApp/DependencyInjectionApp
dotnet run
# Swagger: https://localhost:7xxx/swagger
# Example: GET https://localhost:7xxx/api/weather
```

- KeyedDependencyInjectionApp

```bash
cd FactoryApp/KeyedDependencyInjectionApp
dotnet run
# Swagger: https://localhost:7xxx/swagger
# Example: GET https://localhost:7xxx/api/weather
```

- FactoryApp (lightweight factory)

```bash
cd FactoryApp/FactoryApp
dotnet run
# Swagger: https://localhost:7001/swagger
# Example: GET https://localhost:7001/api/weather?serviceType=TokyoDevUser
```

- AbstractFactoryApp

```bash
cd FactoryApp/AbstractFactoryApp
dotnet run
# Swagger: https://localhost:7xxx/swagger
# Example: GET https://localhost:7xxx/api/weather
```

Note: Exact HTTPS ports may differ; check the console output or `Properties/launchSettings.json` for each project.

## How the samples differ

- DependencyInjectionApp

  - Straightforward DI registrations; controller/use case selects which service to use

- KeyedDependencyInjectionApp

  - Registers multiple implementations keyed by name; resolves by key at runtime

- FactoryApp

  - Uses a small `ClientFactory` to create named `HttpClient`s
  - Adds `LoggingHandler`, `CachingHandler` (HybridCache), and resilience policies via `AddStandardResilienceHandler`
  - Selection is driven by `WeatherServiceType` query string (e.g., `TokyoDevUser`)

- AbstractFactoryApp
  - Encapsulates construction of composed services (retry, caching, auth) behind an abstract factory

## Troubleshooting

- Build errors: ensure you are in the correct directory for the specified project or solution
- Port conflicts: stop any process occupying the dev HTTPS port or change the port in `launchSettings.json`
- Windows "Access is denied" when running:
  - Try running the terminal as Administrator
  - Remove locked binaries under `bin/Debug/net8.0` and rebuild

## License

See `LICENSE`.
