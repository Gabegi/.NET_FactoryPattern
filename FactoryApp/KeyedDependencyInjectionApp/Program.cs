using KeyedDependencyInjectionApp.Application.UseCases;
using KeyedDependencyInjectionApp.Infrastructure;
using KeyedDependencyInjectionApp.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Register HttpClient with configuration from appsettings
builder.Services.AddHttpClient("WeatherApi", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var timeoutSeconds = configuration.GetValue<int>("WeatherApi:TimeoutSeconds", 30);
    var userAgent = configuration.GetValue<string>("WeatherApi:UserAgent", "WeatherApp/1.0");

    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
});

// Keyed dependency injection - register services with keys
builder.Services.AddKeyedScoped<IWeatherService, MockWeatherService>("mock");
builder.Services.AddKeyedScoped<IWeatherService, OpenMeteoService>("openmeteo");

// Register the use case that will receive the keyed service
builder.Services.AddScoped<GetCurrentWeatherUseCase>();
builder.Services.AddLogging();

var app = builder.Build();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
