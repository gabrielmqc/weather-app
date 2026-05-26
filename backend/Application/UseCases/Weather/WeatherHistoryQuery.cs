using Domain.Exceptions;
using Domain.ValueObjects;

namespace Application.UseCases.Weather;

public class WeatherHistoryQuery 
{
    public SearchCriteria SearchBy { get; }
    
    private WeatherHistoryQuery(SearchCriteria criteria)
    {
        SearchBy = criteria;
    }
    
    public static WeatherHistoryQuery ByCity(string city)
    {
        var criteria = CityCriteria.Create(city);
        return new WeatherHistoryQuery(criteria);
    }
    public static WeatherHistoryQuery Create(
        string? city,
        double? lat,
        double? lon)
    {
        var hasCity = !string.IsNullOrWhiteSpace(city);
        var hasCoordinates = lat.HasValue || lon.HasValue;

        if (hasCity && hasCoordinates)
            throw new DomainException("Use city OR coordinates");

        if (!hasCity && (!lat.HasValue || !lon.HasValue))
            throw new DomainException("Inform city or coordinates");

        return hasCity
            ? ByCity(city!)
            : ByCoordinates(lat!.Value, lon!.Value);
    }
    public static WeatherHistoryQuery ByCoordinates(double lat, double lon)
    {
        var criteria = CoordinatesCriteria.Create(lat, lon);
        return new WeatherHistoryQuery(criteria);
    }
}