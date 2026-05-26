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

    [HttpGet("city")]
    public async Task<IActionResult> GetCurrentWeatherByCity(
        [FromQuery] string city)
    {
        var weather =
            await _registerCurrentWeatherByCityNameUseCase
                .Execute(city);

        return Ok(weather);
    }

    [HttpGet("coordinates")]
    public async Task<IActionResult> GetCurrentWeatherByCoordinates(
        [FromQuery] double lat, [FromQuery] double lon)
    {
        var weather =
            await _registerCurrentWeatherByCoordinatesUseCase
                .Execute(lat, lon);

        return Ok(weather);
    }

    [HttpGet("history")]
    public async Task<IActionResult> GetHistory(
        [FromQuery] string? city,
        [FromQuery] double? lat,
        [FromQuery] double? lon)
    {
        var query = WeatherHistoryQuery.Create(city, lat, lon);

        var result = await _getWeatherHistoryUseCase.Execute(query);

        return Ok(result);
    }
}