using Application.DTOs.Response;
using Application.Interfaces.Repositories;
using Application.UseCases.Weather;
using Domain.Entities;
using Domain.ValueObjects.Search;
using FluentAssertions;
using Moq;
using Xunit;

namespace Application.Tests;

public class GetWeatherHistoryUseCaseTests
{
    private readonly Mock<IWeatherRecordRepository> _repositoryMock;
    private readonly GetWeatherHistoryUseCase _sut;

    public GetWeatherHistoryUseCaseTests()
    {
        _repositoryMock = new Mock<IWeatherRecordRepository>();
        _sut = new GetWeatherHistoryUseCase(_repositoryMock.Object);
    }


    [Fact]
    public async Task Execute_WithRecords_ShouldReturnMappedDTOs()
    {
        IReadOnlyList<WeatherRecord> records =
        [
            new("London", 51.5, -0.1, 20.0),
            new("London", 51.5, -0.1, 18.5),
        ];

        _repositoryMock
            .Setup(r => r.GetHistoryAsync(It.IsAny<SearchCriteria>(), It.IsAny<int>()))
            .ReturnsAsync(records);

        var result = await _sut.Execute(WeatherHistoryQuery.ByCity("London"));

        result.Should().HaveCount(2);
        result.Should().AllSatisfy(dto => dto.Should().BeOfType<WeatherRecordDTO>());
    }

    [Fact]
    public async Task Execute_EmptyHistory_ShouldReturnEmptyList()
    {
        _repositoryMock
            .Setup(r => r.GetHistoryAsync(It.IsAny<SearchCriteria>(), It.IsAny<int>()))
            .ReturnsAsync([]);

        var result = await _sut.Execute(WeatherHistoryQuery.ByCity("London"));

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task Execute_ShouldPassCriteriaToRepository()
    {
        _repositoryMock
            .Setup(r => r.GetHistoryAsync(It.IsAny<SearchCriteria>(), It.IsAny<int>()))
            .ReturnsAsync([]);

        var query = WeatherHistoryQuery.ByCity("London");
        await _sut.Execute(query);

        _repositoryMock.Verify(r =>
                r.GetHistoryAsync(
                    It.Is<SearchCriteria>(c => c.Describe() == "City: london"),
                    It.IsAny<int>()),
            Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldMapFieldsCorrectly()
    {
        var record = new WeatherRecord("London", 51.5, -0.1, 20.0);

        _repositoryMock
            .Setup(r => r.GetHistoryAsync(It.IsAny<SearchCriteria>(), It.IsAny<int>()))
            .ReturnsAsync([record]);

        var result = await _sut.Execute(WeatherHistoryQuery.ByCity("London"));

        var dto = result[0];
        dto.Id.Should().Be(record.Id);
        dto.City.Should().Be("London");
        dto.Temperature.Should().Be(20.0);
        dto.Latitude.Should().Be(51.5);
        dto.Longitude.Should().Be(-0.1);
        dto.RecordedAt.Should().Be(record.RecordedAt);
    }
}