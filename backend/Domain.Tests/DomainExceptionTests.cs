using Domain.Exceptions;
using FluentAssertions;
using Xunit;

namespace Domain.Tests;

public class DomainExceptionTests
{
    [Fact]
    public void Constructor_WithMessageOnly_ShouldDefaultTo400()
    {
        var ex = new DomainException("Something went wrong");
 
        ex.Message.Should().Be("Something went wrong");
        ex.StatusCode.Should().Be(400);
    }
 
    [Theory]
    [InlineData(400)]
    [InlineData(404)]
    [InlineData(422)]
    [InlineData(500)]
    public void Constructor_WithExplicitStatusCode_ShouldSetStatusCode(int statusCode)
    {
        var ex = new DomainException("error", statusCode);
 
        ex.StatusCode.Should().Be(statusCode);
    }
 
    [Fact]
    public void DomainException_IsException()
    {
        var ex = new DomainException("error");
 
        ex.Should().BeAssignableTo<Exception>();
    }
}