using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using weather_dashboard_api.DTOs;
using weather_dashboard_api.Services;

namespace weather_dashboard_api.Controllers
{

    [ApiController]
    [Route("api/(controller)")]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;
        private readonly ILogger<WeatherController> _logger;

        public WeatherController(
           IWeatherService weatherService,
           ILogger<WeatherController> logger)
        {
            _weatherService = weatherService;
            _logger = logger;
        }

        [HttpGet("current/{city}")]
        public async Task<ActionResult<CurrentWeatherDto>> GetCurrentWeather(string city)
        {
            _logger.LogInformation("GET request received for weather in city: {City}", city);

            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new { message = "City name is required" });
            }

            try
            {
                var weather = await _weatherService.GetCurrentWeatherAsync(city);

                if (weather == null)
                {
                    _logger.LogWarning("Weather data not found for city: {City}", city);
                    return NotFound(new { message = $"Weather data not found for city: {city}" });
                }

                return Ok(weather);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fetching weather for city: {City}", city);
                return StatusCode(503, new { message = "Weather service temporarily unavailable", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching weather for city: {City}", city);
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }

        }

        [HttpGet("current")]
        public async Task<ActionResult<CurrentWeatherDto>> GetCurrentWeatherByCoordinates(
           [FromQuery] double lat,
           [FromQuery] double lon)
        {
            _logger.LogInformation("GET request received for weather at coordinates: Lat={Lat}, Lon={Lon}", lat, lon);

            if (lat < -90 || lat > 90)
            {
                return BadRequest(new { message = "Latitude must be between -90 and 90" });
            }

            if (lon < -180 || lon > 180)
            {
                return BadRequest(new { message = "Longitude must be between -180 and 180" });
            }

            try
            {
                var weather = await _weatherService.GetCurrentWeatherByCoordinatesAsync(lat, lon);

                if (weather == null)
                {
                    return NotFound(new { message = "Weather data not found for these coordinates" });
                }

                return Ok(weather);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fetching weather for coordinates");
                return StatusCode(503, new { message = "Weather service temporarily unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching weather for coordinates");
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }

        [HttpGet("forecast/{city}")]
        public async Task<ActionResult<ForecastDto>> GetForecast(string city)
        {
            _logger.LogInformation("GET request received for forecast in city: {City}", city);

            if (string.IsNullOrWhiteSpace(city))
            {
                return BadRequest(new { message = "City name is required" });
            }

            try
            {
                var forecast = await _weatherService.GetForecastAsync(city);

                if (forecast == null)
                {
                    _logger.LogWarning("Forecast data not found for city: {City}", city);
                    return NotFound(new { message = $"Forecast data not found for city: {city}" });
                }

                return Ok(forecast);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error fetching forecast for city: {City}", city);
                return StatusCode(503, new { message = "Weather service temporarily unavailable" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching forecast for city: {City}", city);
                return StatusCode(500, new { message = "An unexpected error occurred" });
            }
        }
    }

}

