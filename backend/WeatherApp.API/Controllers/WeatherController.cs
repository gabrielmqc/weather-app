using Application.DTOs.Response;
using Application.UseCases.Weather;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    private readonly RegisterCurrentWeatherByCityNameUseCase _registerCurrentWeatherByCityNameUseCase;
    private readonly RegisterCurrentWeatherByCoordinatesUseCase _registerCurrentWeatherByCoordinatesUseCase;
    private readonly GetWeatherHistoryUseCase _getWeatherHistoryUseCase;
    private readonly GetAllCitiesRegisteredUseCase _getAllCitiesRegisteredUseCase;

    public WeatherController(
        RegisterCurrentWeatherByCityNameUseCase registerCurrentWeatherByCityNameUseCase,
        RegisterCurrentWeatherByCoordinatesUseCase registerCurrentWeatherByCoordinatesUseCase,
        GetWeatherHistoryUseCase getWeatherHistoryUseCase,
        GetAllCitiesRegisteredUseCase getAllCitiesRegisteredUseCase)
    {
        _registerCurrentWeatherByCityNameUseCase = registerCurrentWeatherByCityNameUseCase;
        _registerCurrentWeatherByCoordinatesUseCase = registerCurrentWeatherByCoordinatesUseCase;
        _getWeatherHistoryUseCase = getWeatherHistoryUseCase;
        _getAllCitiesRegisteredUseCase = getAllCitiesRegisteredUseCase;
    }

    [HttpPost("city")]
    public async Task<ActionResult<WeatherRecordDTO>> GetCurrentWeatherByCity(
        [FromQuery] string city)
    {
        WeatherRecordDTO weather =
            await _registerCurrentWeatherByCityNameUseCase
                .Execute(city);

        return Ok(weather);
    }

    [HttpPost("coordinates")]
    public async Task<ActionResult<WeatherRecordDTO>> GetCurrentWeatherByCoordinates(
        [FromQuery] double lat, [FromQuery] double lon)
    {
        WeatherRecordDTO weather =
            await _registerCurrentWeatherByCoordinatesUseCase
                .Execute(lat, lon);

        return Ok(weather);
    }

    [HttpGet("history")]
    public async Task<ActionResult<IReadOnlyList<WeatherRecordDTO>>> GetHistory(
        [FromQuery] string? city,
        [FromQuery] double? lat,
        [FromQuery] double? lon)
    {
        WeatherHistoryQuery query = WeatherHistoryQuery.Create(city, lat, lon);

        IReadOnlyList<WeatherRecordDTO> result = await _getWeatherHistoryUseCase.Execute(query);

        return Ok(result);
    }
    
    [HttpGet("cities")]
    public async Task<ActionResult<IReadOnlyList<WeatherRecordDTO>>> GetCitiesRegistered()
    {

        IReadOnlyList<string> result = await _getAllCitiesRegisteredUseCase.Execute();

        return Ok(result);
    }
}