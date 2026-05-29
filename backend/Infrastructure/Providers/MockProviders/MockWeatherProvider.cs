using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Services;

namespace Infrastructure.Providers.MockProviders;

public class MockWeatherProvider: IWeatherProvider
{
  private static readonly Dictionary<string, (double Lat, double Lon, double Temp)> Cities = 
        new(StringComparer.OrdinalIgnoreCase)
    {
        ["são paulo"] = (-23.5505, -46.6333, 25.5),
        ["sao paulo"] = (-23.5505, -46.6333, 25.5),
        ["rio de janeiro"] = (-22.9068, -43.1729, 30.0),
        ["brasília"] = (-15.7801, -47.9292, 28.0),
        ["brasilia"] = (-15.7801, -47.9292, 28.0),
        ["salvador"] = (-12.9714, -38.5014, 28.0),
        ["manaus"] = (-3.1190, -60.0217, 30.0),
        ["london"] = (51.5074, -0.1278, 20.0),
    };

    public Task<WeatherDataProviderDTO> GetWeatherDataByCityAsync(string city)
    {
        var temp = 20.0;
        var lat = -23.5505;
        var lon = -46.6333;

        if (Cities.TryGetValue(city, out var cityData))
        {
            temp = cityData.Temp;
            lat = cityData.Lat;
            lon = cityData.Lon;
        }

        var weather = new WeatherDataProviderDTO(city, temp, lat, lon);
        return Task.FromResult(weather);
    }

    public Task<WeatherDataProviderDTO> GetWeatherDataByCoordinatesAsync(CoordinatesDTO coordinates)
    {
        var nearestCity = "Localização Desconhecida";
        var closestDistance = double.MaxValue;

        foreach (var city in Cities)
        {
            var distance = CalculateDistance(
                coordinates.Latitude,
                coordinates.Longitude,
                city.Value.Lat,
                city.Value.Lon);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestCity = city.Key;
            }
        }

        var temp = 20.0;
        if (Cities.TryGetValue(nearestCity, out var cityData))
        {
            temp = cityData.Temp;
        }

        var weather = new WeatherDataProviderDTO(
            char.ToUpper(nearestCity[0]) + nearestCity[1..],
            temp,
            coordinates.Latitude,
            coordinates.Longitude);

        return Task.FromResult(weather);
    }

    private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        var R = 6371;
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private double ToRad(double deg) => deg * Math.PI / 180;
}