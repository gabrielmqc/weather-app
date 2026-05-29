namespace Infrastructure.Providers.OpenWeather.DTOs;

public record OpenWeatherGeoResponse(
    string Name,
    double Lat,
    double Lon,
    string Country,
    string State);