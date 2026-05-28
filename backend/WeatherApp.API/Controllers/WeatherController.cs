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

    public WeatherController(
        RegisterCurrentWeatherByCityNameUseCase registerCurrentWeatherByCityNameUseCase,
        RegisterCurrentWeatherByCoordinatesUseCase registerCurrentWeatherByCoordinatesUseCase,
        GetWeatherHistoryUseCase getWeatherHistoryUseCase)
    {
        _registerCurrentWeatherByCityNameUseCase = registerCurrentWeatherByCityNameUseCase;
        _registerCurrentWeatherByCoordinatesUseCase = registerCurrentWeatherByCoordinatesUseCase;
        _getWeatherHistoryUseCase = getWeatherHistoryUseCase;
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
}