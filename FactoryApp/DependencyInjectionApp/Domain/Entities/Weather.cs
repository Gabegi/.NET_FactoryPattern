namespace DependencyInjectionApp.Domain.Entities;

public class Weather
{
    public double Temperature { get; set; }
    public double WindSpeed { get; set; }
    public string Units { get; set; }
    public DateTime Timestamp { get; set; }

    // Domain logic
    public bool IsCold => Temperature < 10;
    public bool IsWarm => Temperature >= 20;
    public bool IsWindy => WindSpeed > 20;
} 