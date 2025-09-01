using FactoryApp.Application.UseCases;
using FactoryApp.Infrastructure.Extensions;
using FactoryApp.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add weather-related services using extension methods
builder.Services.AddWeatherHttpClients();  // This adds both HttpClient and handlers

// Register use case
builder.Services.AddScoped<IWeatherService, GetCurrentWeatherService>();

// Register weather client
builder.Services.AddScoped<IWeatherClient, OpenMeteoClient>();

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
