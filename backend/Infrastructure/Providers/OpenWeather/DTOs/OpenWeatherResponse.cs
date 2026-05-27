namespace Infrastructure.Providers.OpenWeather.DTOs;

public record OpenWeatherResponse(
    Coord Coord,
    MainData Main,
    string Name);

public record Coord(
    double Lon,
    double Lat);

public record MainData(
    double Temp);