using Application.UseCases.Weather;
using Domain.Exceptions;
using Domain.ValueObjects.Search;
using FluentAssertions;
using Xunit;

namespace Application.Tests;

public class WeatherHistoryQueryTests
{
 
    [Fact]
    public void ByCity_ValidCity_ShouldReturnCityCriteria()
    {
        WeatherHistoryQuery query = WeatherHistoryQuery.ByCity("London");
 
        query.SearchBy.Should().BeOfType<CityCriteria>();
        query.SearchBy.Describe().Should().Be("City: London");
    }
 
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void ByCity_InvalidCity_ShouldThrowDomainException(string? city)
    {
        var act = () => WeatherHistoryQuery.ByCity(city!);
 
        act.Should().Throw<DomainException>();
    }
 
 
    [Fact]
    public void ByCoordinates_ValidCoordinates_ShouldReturnCoordinatesCriteria()
    {
        var query = WeatherHistoryQuery.ByCoordinates(48.85, 2.35);
 
        query.SearchBy.Should().BeOfType<CoordinatesCriteria>();
        query.SearchBy.Describe().Should().Be("Lat: 48.85, Lon: 2.35");
    }
 
    [Theory]
    [InlineData(-90.1, 0.0)]
    [InlineData(90.1,  0.0)]
    [InlineData(0.0, -180.1)]
    [InlineData(0.0,  180.1)]
    public void ByCoordinates_InvalidValues_ShouldThrowDomainException(double lat, double lon)
    {
        var act = () => WeatherHistoryQuery.ByCoordinates(lat, lon);
 
        act.Should().Throw<DomainException>();
    }
 
 
    [Fact]
    public void Create_OnlyCityProvided_ShouldReturnCityCriteria()
    {
        var query = WeatherHistoryQuery.Create("London", null, null);
 
        query.SearchBy.Should().BeOfType<CityCriteria>();
    }
 
    [Fact]
    public void Create_OnlyCoordinatesProvided_ShouldReturnCoordinatesCriteria()
    {
        var query = WeatherHistoryQuery.Create(null, 48.85, 2.35);
 
        query.SearchBy.Should().BeOfType<CoordinatesCriteria>();
    }
    
    [Fact]
    public void Create_BothCityAndCoordinates_ShouldThrowDomainException()
    {
        var act = () => WeatherHistoryQuery.Create("London", 51.5, -0.1);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Use city OR coordinates");
    }
 
    [Fact]
    public void Create_NothingProvided_ShouldThrowDomainException()
    {
        var act = () => WeatherHistoryQuery.Create(null, null, null);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Inform city or coordinates");
    }
 
    [Fact]
    public void Create_OnlyLatProvided_ShouldThrowDomainException()
    {
        var act = () => WeatherHistoryQuery.Create(null, 48.85, null);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Inform city or coordinates");
    }
 
    [Fact]
    public void Create_OnlyLonProvided_ShouldThrowDomainException()
    {
        var act = () => WeatherHistoryQuery.Create(null, null, 2.35);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Inform city or coordinates");
    }
 
    [Theory]
    [InlineData("   ")]
    [InlineData("")]
    public void Create_WhitespaceCityNoCoordinates_ShouldThrowDomainException(string city)
    {
        var act = () => WeatherHistoryQuery.Create(city, null, null);
 
        act.Should().Throw<DomainException>();
    }
}