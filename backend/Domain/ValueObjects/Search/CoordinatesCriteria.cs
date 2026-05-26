using Domain.Utils;

namespace Domain.ValueObjects;

public record CoordinatesCriteria(double Latitude, double Longitude) : SearchCriteria
{
    public override string Describe() => $"Lat: {Latitude}, Lon: {Longitude}";
        
    public static CoordinatesCriteria Create(double lat, double lon)
    {
        Guard.AgainstOutOfRange(lat, -90, 90, "Invalid latitude");
        Guard.AgainstOutOfRange(lon, -180, 180, "Invalid longitude");
        return new CoordinatesCriteria(lat, lon);
    }
}