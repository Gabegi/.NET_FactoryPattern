using FactoryApp.Application.UseCases;
using FactoryApp.Infrastructure;
using FactoryApp.Infrastructure.Config;
using FactoryApp.Infrastructure.Factories;
using FactoryApp.Infrastructure.Interfaces;
using FactoryApp.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add weather-related services using extension methods
builder.Services.AddWeatherHttpClients();  // This adds both HttpClients and caching
builder.Services.AddWeatherServices();     // This adds your business services

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
