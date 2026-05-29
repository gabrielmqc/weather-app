using System.Globalization;
using Domain.Utils;

namespace Domain.ValueObjects.Search;

public record CoordinatesCriteria(double Latitude, double Longitude) : SearchCriteria
{
    public override string Describe() =>
        string.Format(
            CultureInfo.InvariantCulture,
            "Lat: {0:F2}, Lon: {1:F2}",
            Latitude,
            Longitude);
        
    public static CoordinatesCriteria Create(double lat, double lon)
    {
        Guard.AgainstOutOfRange(lat, -90, 90, "Invalid latitude");
        Guard.AgainstOutOfRange(lon, -180, 180, "Invalid longitude");
        return new CoordinatesCriteria(lat, lon);
    }
}