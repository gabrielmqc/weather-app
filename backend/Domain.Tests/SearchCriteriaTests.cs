using Domain.Exceptions;
using Domain.ValueObjects.Search;
using FluentAssertions;
using Xunit;

namespace Domain.Tests;

public class SearchCriteriaTests
{
 
    [Fact]
    public void Create_ValidCity_ShouldReturnCityCriteria()
    {
        var criteria = CityCriteria.Create("Tokyo");
 
        criteria.City.Should().Be("Tokyo");
    }
 
    [Fact]
    public void Describe_ShouldReturnFormattedStringCity()
    {
        var criteria = CityCriteria.Create("Tokyo");
 
        criteria.Describe().Should().Be("City: Tokyo");
    }
 
    [Fact]
    public void Create_SameCity_ShouldBeEqual()
    {
        var a = CityCriteria.Create("Tokyo");
        var b = CityCriteria.Create("Tokyo");
 
        a.Should().Be(b);
    }
 
    [Fact]
    public void Create_DifferentCities_ShouldNotBeEqual()
    {
        var a = CityCriteria.Create("Tokyo");
        var b = CityCriteria.Create("Seoul");
 
        a.Should().NotBe(b);
    }
 
 
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void Create_NullOrWhiteSpaceCity_ShouldThrowDomainException(string? city)
    {
        var act = () => CityCriteria.Create(city!);
 
        act.Should().Throw<DomainException>()
           .WithMessage("City cannot be empty");
    }
 
 
    [Fact]
    public void CityCriteria_IsSearchCriteria()
    {
        var criteria = CityCriteria.Create("Tokyo");
 
        criteria.Should().BeAssignableTo<SearchCriteria>();
    }
 
    [Fact]
    public void Create_ValidCoordinates_ShouldReturnCoordinatesCriteria()
    {
        var criteria = CoordinatesCriteria.Create(35.68, 139.69);
 
        criteria.Latitude.Should().Be(35.68);
        criteria.Longitude.Should().Be(139.69);
    }
 
    [Fact]
    public void Describe_ShouldReturnFormattedString()
    {
        var criteria = CoordinatesCriteria.Create(35.68, 139.69);
 
        criteria.Describe().Should().Be("Lat: 35.68, Lon: 139.69");
    }
 
    [Fact]
    public void Create_SameCoordinates_ShouldBeEqual()
    {
        var a = CoordinatesCriteria.Create(10.0, 20.0);
        var b = CoordinatesCriteria.Create(10.0, 20.0);
 
        a.Should().Be(b);
    }
 
    [Fact]
    public void Create_DifferentCoordinates_ShouldNotBeEqual()
    {
        var a = CoordinatesCriteria.Create(10.0, 20.0);
        var b = CoordinatesCriteria.Create(11.0, 21.0);
 
        a.Should().NotBe(b);
    }
 
 
    [Theory]
    [InlineData(-90.0,    0.0)]
    [InlineData( 90.0,    0.0)]
    [InlineData(  0.0, -180.0)]
    [InlineData(  0.0,  180.0)]
    public void Create_BoundaryCoordinates_ShouldNotThrow(double lat, double lon)
    {
        var act = () => CoordinatesCriteria.Create(lat, lon);
 
        act.Should().NotThrow();
    }
    
    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    public void Create_InvalidLatitude_ShouldThrowDomainException(double latitude)
    {
        var act = () => CoordinatesCriteria.Create(latitude, 0.0);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid latitude");
    }
    
    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    public void Create_InvalidLongitude_ShouldThrowDomainException(double longitude)
    {
        var act = () => CoordinatesCriteria.Create(0.0, longitude);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid longitude");
    }
    
    [Fact]
    public void CoordinatesCriteria_IsSearchCriteria()
    {
        var criteria = CoordinatesCriteria.Create(0.0, 0.0);
 
        criteria.Should().BeAssignableTo<SearchCriteria>();
    }
}
 

    

