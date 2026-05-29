using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Tests;

public class WeatherRecordTests
{
 
    [Fact]
    public void Constructor_ValidArguments_ShouldCreateRecord()
    {
        DateTime before = DateTime.UtcNow;
 
        var record = new WeatherRecord("London", 51.5, -0.1, 20.0);
 
        record.City.Should().Be("London");
        record.Latitude.Should().Be(51.5);
        record.Longitude.Should().Be(-0.1);
        record.Temperature.Should().Be(20.0);
        record.Id.Should().NotBe(Guid.Empty);
        record.RecordedAt.Should().BeOnOrAfter(before).And.BeOnOrBefore(DateTime.UtcNow);
    }
 
    [Fact]
    public void Constructor_CalledTwice_ShouldGenerateDifferentIds()
    {
        var a = new WeatherRecord("London", 51.5, -0.1, 20.0);
        var b = new WeatherRecord("Paris",  48.8,  2.3, 18.0);
 
        a.Id.Should().NotBe(b.Id);
    }
 
    [Theory]
    [InlineData(-90.0)]   
    [InlineData(0.0)]
    [InlineData(90.0)]    
    public void Constructor_BoundaryLatitude_ShouldNotThrow(double latitude)
    {
        var act = () => new WeatherRecord("City", latitude, 0.0, 15.0);
        act.Should().NotThrow();
    }
 
    [Theory]
    [InlineData(-180.0)]  
    [InlineData(0.0)]
    [InlineData(180.0)] 
    public void Constructor_BoundaryLongitude_ShouldNotThrow(double longitude)
    {
        var act = () => new WeatherRecord("City", 0.0, longitude, 15.0);
        act.Should().NotThrow();
    }
    
 
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void Constructor_NullOrWhiteSpaceCity_ShouldThrowDomainException(string? city)
    {
        var act = () => new WeatherRecord(city!, 0.0, 0.0, 15.0);
 
        act.Should().Throw<DomainException>()
           .WithMessage("City cannot be empty.");
    }
 
 
    [Theory]
    [InlineData(-90.1)]
    [InlineData(90.1)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public void Constructor_InvalidLatitude_ShouldThrowDomainException(double latitude)
    {
        var act = () => new WeatherRecord("City", latitude, 0.0, 15.0);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Latitude is invalid.");
    }
 
 
    [Theory]
    [InlineData(-180.1)]
    [InlineData(180.1)]
    [InlineData(double.MaxValue)]
    [InlineData(double.MinValue)]
    public void Constructor_InvalidLongitude_ShouldThrowDomainException(double longitude)
    {
        var act = () => new WeatherRecord("City", 0.0, longitude, 15.0);
 
        act.Should().Throw<DomainException>()
           .WithMessage("Longitude is invalid.");
    }
}