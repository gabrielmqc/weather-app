using Domain.Exceptions;
using Domain.ValueObjects.Commands;
using FluentAssertions;
using Xunit;

namespace Domain.Tests;

public class LocationTests
{
    [Fact]
    public void Create_ValidName_ShouldReturnCityLocation()
    {
        var location = CityLocation.Create("London");

        location.Name.Should().Be("London");
    }

    [Fact]
    public void Describe_ShouldReturnFormattedString()
    {
        var location = CityLocation.Create("London");

        location.Describe().Should().Be("City: London");
    }

    [Fact]
    public void Create_SameName_ShouldBeEqual()
    {
        var a = CityLocation.Create("Paris");
        var b = CityLocation.Create("Paris");

        a.Should().Be(b);
    }

    [Fact]
    public void Create_DifferentNames_ShouldNotBeEqual()
    {
        var a = CityLocation.Create("Paris");
        var b = CityLocation.Create("London");

        a.Should().NotBe(b);
    }


    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void Create_NullOrWhiteSpaceName_ShouldThrowDomainException(string? name)
    {
        var act = () => CityLocation.Create(name!);

        act.Should().Throw<DomainException>()
            .WithMessage("City name cannot be empty");
    }


    [Fact]
    public void CityLocation_IsLocation()
    {
        var location = CityLocation.Create("Berlin");

        location.Should().BeAssignableTo<Location>();
    }

    [Fact]
    public void Create_ValidCoordinates_ShouldReturnCoordinatesLocation()
    {
        var location = CoordinatesLocation.Create(48.85, 2.35);

        location.Latitude.Should().Be(48.85);
        location.Longitude.Should().Be(2.35);
    }

    [Fact]
    public void Describe_ShouldReturnFormattedStringCoordinates()
    {
        var location = CoordinatesLocation.Create(48.85, 2.35);

        location.Describe().Should().Be("Lat: 48.85, Lon: 2.35");
    }

    [Fact]
    public void Create_SameCoordinates_ShouldBeEqual()
    {
        var a = CoordinatesLocation.Create(10.0, 20.0);
        var b = CoordinatesLocation.Create(10.0, 20.0);

        a.Should().Be(b);
    }

    [Fact]
    public void Create_DifferentCoordinates_ShouldNotBeEqual()
    {
        var a = CoordinatesLocation.Create(10.0, 20.0);
        var b = CoordinatesLocation.Create(11.0, 21.0);

        a.Should().NotBe(b);
    }


    [Theory]
    [InlineData(-90.0, 0.0)]
    [InlineData(90.0, 0.0)]
    [InlineData(0.0, -180.0)]
    [InlineData(0.0, 180.0)]
    public void Create_BoundaryValues_ShouldNotThrow(double lat, double lon)
    {
        var act = () => CoordinatesLocation.Create(lat, lon);

        act.Should().NotThrow();
    }


    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    public void Create_InvalidLatitude_ShouldThrowDomainException(double latitude)
    {
        var act = () => CoordinatesLocation.Create(latitude, 0.0);

        act.Should().Throw<DomainException>()
            .WithMessage("Invalid latitude");
    }


    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    public void Create_InvalidLongitude_ShouldThrowDomainException(double longitude)
    {
        var act = () => CoordinatesLocation.Create(0.0, longitude);

        act.Should().Throw<DomainException>()
            .WithMessage("Invalid longitude");
    }


    [Fact]
    public void CoordinatesLocation_IsLocation()
    {
        var location = CoordinatesLocation.Create(0.0, 0.0);

        location.Should().BeAssignableTo<Location>();
    }
}