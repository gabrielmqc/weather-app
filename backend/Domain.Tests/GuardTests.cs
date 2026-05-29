using Domain.Exceptions;
using Domain.Utils;
using FluentAssertions;
using Xunit;

namespace Domain.Tests;

public class GuardTests
{
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public void AgainstNullOrWhiteSpace_InvalidValue_ShouldThrowDomainException(string? value)
    {
        var act = () => Guard.AgainstNullOrWhiteSpace(value!, "error message");
 
        act.Should().Throw<DomainException>()
           .WithMessage("error message");
    }
 
    [Theory]
    [InlineData("valid")]
    [InlineData("  valid  ")]
    [InlineData("a")]
    public void AgainstNullOrWhiteSpace_ValidValue_ShouldNotThrow(string value)
    {
        var act = () => Guard.AgainstNullOrWhiteSpace(value, "error message");
 
        act.Should().NotThrow();
    }
 
    [Fact]
    public void AgainstNullOrWhiteSpace_ThrowsWithExactMessage()
    {
        var act = () => Guard.AgainstNullOrWhiteSpace("", "Custom error");
 
        act.Should().Throw<DomainException>()
           .Which.Message.Should().Be("Custom error");
    }
 
 
    [Theory]
    [InlineData(0.0,  -10.0, 10.0)]   // middle
    [InlineData(-10.0, -10.0, 10.0)]  // exactly min
    [InlineData(10.0,  -10.0, 10.0)]  // exactly max
    public void AgainstOutOfRange_ValueInRange_ShouldNotThrow(double value, double min, double max)
    {
        var act = () => Guard.AgainstOutOfRange(value, min, max, "out of range");
 
        act.Should().NotThrow();
    }
 
    [Theory]
    [InlineData(-10.1, -10.0, 10.0)]
    [InlineData(10.1,  -10.0, 10.0)]
    [InlineData(double.MaxValue, -10.0, 10.0)]
    [InlineData(double.MinValue, -10.0, 10.0)]
    public void AgainstOutOfRange_ValueOutOfRange_ShouldThrowDomainException(double value, double min, double max)
    {
        var act = () => Guard.AgainstOutOfRange(value, min, max, "out of range");
 
        act.Should().Throw<DomainException>()
           .WithMessage("out of range");
    }
 
    [Fact]
    public void AgainstOutOfRange_ThrowsWithExactMessage()
    {
        var act = () => Guard.AgainstOutOfRange(999, 0, 10, "Custom range error");
 
        act.Should().Throw<DomainException>()
           .Which.Message.Should().Be("Custom range error");
    }
}