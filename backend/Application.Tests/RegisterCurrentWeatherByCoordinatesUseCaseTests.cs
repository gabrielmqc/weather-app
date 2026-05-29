using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.UseCases.Weather;
using Domain.Entities;
using Domain.Exceptions;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests;

public class RegisterCurrentWeatherByCoordinatesUseCaseTests
{
    private readonly Mock<IWeatherProviderFactory> _weatherProviderFactoryMock;
    private readonly Mock<IWeatherProvider> _weatherProviderMock;
    private readonly Mock<IWeatherRecordRepository> _repositoryMock;
    private readonly RegisterCurrentWeatherByCoordinatesUseCase _sut;

    public RegisterCurrentWeatherByCoordinatesUseCaseTests()
    {
        
        _weatherProviderMock = new Mock<IWeatherProvider>();
        _weatherProviderFactoryMock = new Mock<IWeatherProviderFactory>();
        _repositoryMock = new Mock<IWeatherRecordRepository>();
        
        _weatherProviderFactoryMock
            .Setup(f => f.GetProvider())
            .Returns(_weatherProviderMock.Object);
        
        _sut = new RegisterCurrentWeatherByCoordinatesUseCase(
            _weatherProviderFactoryMock.Object,
            _repositoryMock.Object);
    }


    [Fact]
    public async Task Execute_ValidCoordinates_ShouldReturnWeatherRecordDTO()
    {
        var providerDto = new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1);

        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(providerDto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        var result = await _sut.Execute(51.5, -0.1);

        result.Should().BeOfType<WeatherRecordDTO>();
        result.City.Should().Be("London");
        result.Temperature.Should().Be(20.0);
        result.Latitude.Should().Be(51.5);
        result.Longitude.Should().Be(-0.1);
    }

    [Fact]
    public async Task Execute_ValidCoordinates_ShouldCallProviderWithCorrectCoordinates()
    {
        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1));

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        await _sut.Execute(51.5, -0.1);

        _weatherProviderMock.Verify(p =>
                p.GetWeatherDataByCoordinatesAsync(
                    It.Is<CoordinatesDTO>(c => c.Latitude == 51.5 && c.Longitude == -0.1)),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ValidCoordinates_ShouldPersistRecord()
    {
        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1));

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        await _sut.Execute(51.5, -0.1);

        _repositoryMock.Verify(r =>
                r.AddAsync(It.Is<WeatherRecord>(wr =>
                    wr.City == "London" &&
                    wr.Latitude == 51.5 &&
                    wr.Longitude == -0.1)),
            Times.Once);
    }


    [Theory]
    [InlineData(-90.0, 0.0)]
    [InlineData(90.0, 0.0)]
    [InlineData(0.0, -180.0)]
    [InlineData(0.0, 180.0)]
    public async Task Execute_BoundaryCoordinates_ShouldNotThrow(double lat, double lon)
    {
        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()))
            .ReturnsAsync(new WeatherDataProviderDTO("City", 15.0, lat, lon));

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        var act = async () => await _sut.Execute(lat, lon);

        await act.Should().NotThrowAsync();
    }


    [Theory]
    [InlineData(-90.1, 0.0)]
    [InlineData(90.1, 0.0)]
    [InlineData(0.0, -180.1)]
    [InlineData(0.0, 180.1)]
    public async Task Execute_InvalidCoordinates_ShouldThrowDomainExceptionBeforeCallingProvider(double lat, double lon)
    {
        var act = async () => await _sut.Execute(lat, lon);

        await act.Should().ThrowAsync<DomainException>();

        _weatherProviderMock
            .Verify(p => p.GetWeatherDataByCoordinatesAsync(It.IsAny<CoordinatesDTO>()), Times.Never);

        _repositoryMock
            .Verify(r => r.AddAsync(It.IsAny<WeatherRecord>()), Times.Never);
    }
}