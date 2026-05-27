using Application.DTOs.Response;
using Application.UseCases.Location;
using Application.UseCases.Weather;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/location")]
public class LocationController : ControllerBase
{
    private readonly GetCityLocationsByNameUsecase _getCityLocationsByNameUseCase;

    public LocationController(GetCityLocationsByNameUsecase getCityLocationsByNameUseCase)
    {
        _getCityLocationsByNameUseCase = getCityLocationsByNameUseCase;
    }

    [HttpGet("city")]
    public async Task<ActionResult<List<CoordinatesResponseDTO>>> GetCurrentWeatherByCity(
        [FromQuery] string city)
    {
        List<CoordinatesResponseDTO> locations = await _getCityLocationsByNameUseCase.Execute(city);

        return Ok(locations);
    }
}