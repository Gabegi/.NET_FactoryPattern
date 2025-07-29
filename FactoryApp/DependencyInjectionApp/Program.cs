using DependencyInjectionApp.Domain.Interfaces;
using DependencyInjectionApp.Infrastructure.Services;
using DependencyInjectionApp.Application.UseCases;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddControllers();

// Add Swagger (optional but recommended for API testing)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register HttpClient for dependency injection
builder.Services.AddHttpClient();

// Register repositories (Infrastructure layer)
builder.Services.AddScoped<IWeatherRepository, OpenMeteoWeatherRepository>();

// Register use cases (Application layer)
builder.Services.AddScoped<GetCurrentWeatherUseCase>();

// To use mock repository instead:
// builder.Services.AddScoped<IWeatherRepository, MockWeatherRepository>();

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
