using Application.DTOs.Response;
using Application.Interfaces.Services;
using Application.UseCases.Location;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests;

public class GetCityLocationByNameUseCaseTests
{
    private readonly Mock<ICoordinatesProvider> _coordinatesProviderMock;
    private readonly GetCityLocationsByNameUsecase _sut;

    public GetCityLocationByNameUseCaseTests()
    {
        _coordinatesProviderMock = new Mock<ICoordinatesProvider>();
        _sut = new GetCityLocationsByNameUsecase(_coordinatesProviderMock.Object);
    }


    [Fact]
    public async Task Execute_ValidCity_ShouldReturnResults()
    {
        var expected = new List<CoordinatesResponseDTO>
        {
            new("London", 51.5, -0.1, "GB", "England"),
        };

        _coordinatesProviderMock
            .Setup(p => p.GetCoordinatesByCityAsync("London"))
            .ReturnsAsync(expected);

        var result = await _sut.Execute("London");

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task Execute_ValidCity_ShouldCallProviderWithExactName()
    {
        _coordinatesProviderMock
            .Setup(p => p.GetCoordinatesByCityAsync(It.IsAny<string>()))
            .ReturnsAsync([]);

        await _sut.Execute("Paris");

        _coordinatesProviderMock
            .Verify(p => p.GetCoordinatesByCityAsync("Paris"), Times.Once);
    }

    [Fact]
    public async Task Execute_ProviderReturnsEmpty_ShouldReturnEmpty()
    {
        _coordinatesProviderMock
            .Setup(p => p.GetCoordinatesByCityAsync(It.IsAny<string>()))
            .ReturnsAsync([]);

        var result = await _sut.Execute("UnknownCity");

        result.Should().BeEmpty();
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public async Task Execute_InvalidCity_ShouldThrowDomainExceptionBeforeCallingProvider(string? city)
    {
        var act = async () => await _sut.Execute(city!);

        await act.Should().ThrowAsync<DomainException>();

        _coordinatesProviderMock
            .Verify(p => p.GetCoordinatesByCityAsync(It.IsAny<string>()), Times.Never);
    }
}