using System.Globalization;
using System.Text;
using Domain.Utils;

namespace Domain.Entities;

public class WeatherRecord
{
    public Guid Id { get; private init; }
    
    public string City { get; private set; } = string.Empty;
    
    public string NormalizedCity  { get; private set; } = string.Empty;

    public double Latitude { get; private set; }

    public double Longitude { get; private set; }

    public double Temperature { get; private set; }

    public DateTime RecordedAt { get; private set; }
    
    private WeatherRecord()
    {
    }

    public WeatherRecord(
        string city,
        double latitude,
        double longitude,
        double temperature)
    {
        
        Id = Guid.NewGuid();

        City = city.Trim();
        NormalizedCity = StringNormalizer.Normalize(city);
        Latitude = latitude;
        Longitude = longitude;
        Temperature = temperature;

        RecordedAt = DateTime.UtcNow;

        Validate();
    }

    private void Validate()
    {
        Guard.AgainstNullOrWhiteSpace(
            City,
            "City cannot be empty.");
        
        Guard.AgainstOutOfRange(
            Latitude,
            -90,
            90,
            "Latitude is invalid.");

        Guard.AgainstOutOfRange(
            Longitude,
            -180,
            180,
            "Longitude is invalid.");
    }

}