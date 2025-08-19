using FactoryApp.Application.UseCases;
using FactoryApp.Infrastructure;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient with configuration from appsettings
builder.Services.AddHttpClient("WeatherApi", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var timeoutSeconds = configuration.GetValue<int>("WeatherApi:TimeoutSeconds", 30);
    var userAgent = configuration.GetValue<string>("WeatherApi:UserAgent", "WeatherApp/1.0");

    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    client.DefaultRequestHeaders.Add("User-Agent", userAgent);
});


// HttpClient WITH caching
builder.Services.AddHttpClient("WeatherApiCached", (serviceProvider, client) =>
{
    var configuration = serviceProvider.GetRequiredService<IConfiguration>();
    var timeoutSeconds = configuration.GetValue<int>("WeatherApi:TimeoutSeconds", 30);
    var userAgent = configuration.GetValue<string>("WeatherApi:UserAgent", "WeatherApp/1.0");
    var baseUrl = configuration.GetValue<string>("WeatherApi:BaseUrl", "");

    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    client.DefaultRequestHeaders.Add("User-Agent", userAgent);

    if (!string.IsNullOrEmpty(baseUrl))
    {
        client.BaseAddress = new Uri(baseUrl);
    }
})
.AddHttpMessageHandler<WeatherCachingHandler>(); // Add caching handler

// Register the caching handler
builder.Services.AddTransient<WeatherCachingHandler>();

// Register weather services
builder.Services.AddScoped<MockWeatherClient>();
builder.Services.AddScoped<OpenMeteoClient>();

// Register cache and retry services
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();
builder.Services.AddScoped<IRetryPolicyService, SimpleRetryPolicyService>();

// Register factory
builder.Services.AddScoped<IWeatherClientFactory, WeatherClientFactory>();

// Register use case
builder.Services.AddScoped<GetCurrentWeatherService>();

// Register logging
builder.Services.AddLogging();

var app = builder.Build();

// Enable Swagger in Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
