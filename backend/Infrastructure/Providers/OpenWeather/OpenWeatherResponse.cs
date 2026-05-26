namespace Infrastructure.Providers.OpenWeather;

public class OpenWeatherResponse
{
    public Coord Coord { get; set; } = default!;

    public MainData Main { get; set; } = default!;

    public string Name { get; set; } = string.Empty;
}

public class Coord
{
    public double Lon { get; set; }

    public double Lat { get; set; }
}

public class MainData
{
    public double Temp { get; set; }
}