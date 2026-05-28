using Application.DTOs.Response;
using Application.Mappers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.Tests;

public class WeatherMapperTests
{
    private static WeatherRecord MakeRecord() =>
        new("London", 51.5, -0.1, 20.0);
 
    private static WeatherDataProviderDTO MakeDto() =>
        new("London", 20.0, 51.5, -0.1);
 
 
    [Fact]
    public void FromEntityToResponseDTO_ShouldMapAllFields()
    {
        var record = MakeRecord();
 
        var dto = WeatherMapper.FromEntityToResponseDTO(record);
 
        dto.Id.Should().Be(record.Id);
        dto.City.Should().Be(record.City);
        dto.Temperature.Should().Be(record.Temperature);
        dto.Latitude.Should().Be(record.Latitude);
        dto.Longitude.Should().Be(record.Longitude);
        dto.RecordedAt.Should().Be(record.RecordedAt);
    }
 
    [Fact]
    public void FromEntityToResponseDTO_ShouldReturnWeatherRecordDTO()
    {
        var dto = WeatherMapper.FromEntityToResponseDTO(MakeRecord());
 
        dto.Should().BeOfType<WeatherRecordDTO>();
    }
 
 
    [Fact]
    public void ToEntity_ShouldMapAllFields()
    {
        var dto = MakeDto();
 
        var entity = WeatherMapper.ToEntity(dto);
 
        entity.City.Should().Be(dto.City);
        entity.Temperature.Should().Be(dto.Temperature);
        entity.Latitude.Should().Be(dto.Latitude);
        entity.Longitude.Should().Be(dto.Longitude);
    }
 
    [Fact]
    public void ToEntity_ShouldReturnWeatherRecord()
    {
        var entity = WeatherMapper.ToEntity(MakeDto());
 
        entity.Should().BeOfType<WeatherRecord>();
    }
 
    [Fact]
    public void ToEntity_ShouldAssignNewGuid()
    {
        var entity = WeatherMapper.ToEntity(MakeDto());
 
        entity.Id.Should().NotBe(Guid.Empty);
    }
    
    [Fact]
    public void FromEntityListToResponseDTOList_ShouldMapAllItems()
    {
        IReadOnlyList<WeatherRecord> records =
        [
            new("London", 51.5, -0.1, 20.0),
            new("Paris",  48.8,  2.3, 18.0),
        ];
 
        var dtos = WeatherMapper.FromEntityListToResponseDTOList(records);
 
        dtos.Should().HaveCount(2);
        dtos[0].City.Should().Be("London");
        dtos[1].City.Should().Be("Paris");
    }
 
    [Fact]
    public void FromEntityListToResponseDTOList_EmptyList_ShouldReturnEmptyList()
    {
        var dtos = WeatherMapper.FromEntityListToResponseDTOList([]);
 
        dtos.Should().BeEmpty();
    }
}