using Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace WeatherApp.API.Controllers;

[ApiController]
[Route("api/weather")]
public class WeatherController : ControllerBase
{
    
    private readonly IWeatherProvider _weatherProvider;

    public WeatherController(
        IWeatherProvider weatherProvider)
    {
        _weatherProvider = weatherProvider;
    }

    [HttpGet("city")]
    public async Task<IActionResult> GetCurrentWeatherByCity(
        [FromQuery] string city)
    {
        var weather =
            await _weatherProvider
                .GetWeatherDataByCityAsync(city);

        return Ok(weather);
    }
}