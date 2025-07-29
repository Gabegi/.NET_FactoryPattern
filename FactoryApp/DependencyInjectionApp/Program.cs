using DependencyInjectionApp.Application.Interfaces;
using DependencyInjectionApp.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

// Add Swagger (optional but recommended for API testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

// Register the weather service using dependency injection
builder.Services.AddScoped<IWeatherService, OpenMeteoService>();

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
