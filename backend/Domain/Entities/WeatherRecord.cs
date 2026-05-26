using Domain.Utils;

namespace Domain.Entities;

public class WeatherRecord
{
    public Guid Id { get; private init; }
    
    public string City { get; private set; } = string.Empty;

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public decimal Temperature { get; private set; }

    public DateTime RecordedAt { get; private set; }
    
    private WeatherRecord()
    {
    }

    public WeatherRecord(
        string city,
        double latitude,
        double longitude,
        decimal temperature)
    {
        Guard.AgainstNullOrWhiteSpace(
            city,
            "City cannot be empty.");

        Guard.AgainstOutOfRange(
            latitude,
            -90,
            90,
            "Latitude is invalid.");

        Guard.AgainstOutOfRange(
            longitude,
            -180,
            180,
            "Longitude is invalid.");
        Id = Guid.NewGuid();

        City = city;
        Latitude = latitude;
        Longitude = longitude;
        Temperature = temperature;

        RecordedAt = DateTime.UtcNow;
    }
}