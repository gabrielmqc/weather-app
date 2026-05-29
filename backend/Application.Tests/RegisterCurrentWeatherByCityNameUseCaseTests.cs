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

public class RegisterCurrentWeatherByCityNameUseCaseTests
{
    private readonly Mock<IWeatherProviderFactory> _weatherProviderFactoryMock;
    private readonly Mock<IWeatherProvider> _weatherProviderMock;
    private readonly Mock<IWeatherRecordRepository> _repositoryMock;
    private readonly RegisterCurrentWeatherByCityNameUseCase _sut;

    public RegisterCurrentWeatherByCityNameUseCaseTests()
    {
        _weatherProviderMock = new Mock<IWeatherProvider>();
        _weatherProviderFactoryMock = new Mock<IWeatherProviderFactory>();
        _repositoryMock = new Mock<IWeatherRecordRepository>();
        
        _weatherProviderFactoryMock
            .Setup(f => f.GetProvider())
            .Returns(_weatherProviderMock.Object);
        
        _sut = new RegisterCurrentWeatherByCityNameUseCase(
            _weatherProviderFactoryMock.Object,
            _repositoryMock.Object);
    }


    [Fact]
    public async Task Execute_ValidCity_ShouldReturnWeatherRecordDTO()
    {
        var providerDto = new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1);

        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCityAsync("London"))
            .ReturnsAsync(providerDto);

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        var result = await _sut.Execute("London");

        result.Should().BeOfType<WeatherRecordDTO>();
        result.City.Should().Be("London");
        result.Temperature.Should().Be(20.0);
        result.Latitude.Should().Be(51.5);
        result.Longitude.Should().Be(-0.1);
    }

    [Fact]
    public async Task Execute_ValidCity_ShouldPersistRecord()
    {
        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCityAsync(It.IsAny<string>()))
            .ReturnsAsync(new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1));

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        await _sut.Execute("London");

        _repositoryMock.Verify(r =>
                r.AddAsync(It.Is<WeatherRecord>(wr => wr.City == "London")),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ValidCity_ShouldCallProviderWithExactName()
    {
        _weatherProviderMock
            .Setup(p => p.GetWeatherDataByCityAsync(It.IsAny<string>()))
            .ReturnsAsync(new WeatherDataProviderDTO("London", 20.0, 51.5, -0.1));

        _repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<WeatherRecord>()))
            .ReturnsAsync((WeatherRecord r) => r);

        await _sut.Execute("London");

        _weatherProviderMock
            .Verify(p => p.GetWeatherDataByCityAsync("London"), Times.Once);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null!)]
    public async Task Execute_InvalidCity_ShouldThrowDomainExceptionBeforeCallingProvider(string? city)
    {
        var act = async () => await _sut.Execute(city!);

        await act.Should().ThrowAsync<DomainException>();

        _weatherProviderMock
            .Verify(p => p.GetWeatherDataByCityAsync(It.IsAny<string>()), Times.Never);

        _repositoryMock
            .Verify(r => r.AddAsync(It.IsAny<WeatherRecord>()), Times.Never);
    }
}