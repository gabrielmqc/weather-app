using Domain.Utils;

namespace Domain.ValueObjects.Commands;

public record CoordinatesLocation : Location
{
    public double Latitude { get; }
    public double Longitude { get; }
        
    private CoordinatesLocation(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }
        
    public static CoordinatesLocation Create(double latitude, double longitude)
    {
        Guard.AgainstOutOfRange(latitude, -90, 90, "Invalid latitude");
        Guard.AgainstOutOfRange(longitude, -180, 180, "Invalid longitude");
        return new CoordinatesLocation(latitude, longitude);
    }
        
    public override string Describe() => $"Lat: {Latitude}, Lon: {Longitude}";
}