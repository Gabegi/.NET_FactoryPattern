namespace DependencyInjectionApp.Domain.Entities;

public class Weather
{
    public double Temperature { get; private set; }
    public double WindSpeed { get; private set; }
    public string Units { get; private set; }
    public DateTime Timestamp { get; private set; }

    // Domain logic
    public bool IsCold => Temperature < 10;
    public bool IsWarm => Temperature >= 20;
    public bool IsWindy => WindSpeed > 20;
} 