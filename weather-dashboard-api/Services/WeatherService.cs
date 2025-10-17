using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Text.Json;
using weather_dashboard_api.DTOs;
using weather_dashboard_api.Models;

namespace weather_dashboard_api.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/";

        public WeatherService(HttpClient httpClient,
            IMemoryCache memoryCache,
            ILogger<WeatherService> logger,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _cache = memoryCache;
            _logger = logger;
            _apiKey = configuration["OpenWeatherMap:ApiKey"]
                ?? throw new InvalidOperationException("openWeatherMap API key not ");
            _httpClient.BaseAddress=new Uri(_baseUrl);
        }

        public async Task<CurrentWeatherDto?> GetCurrentWeatherAsync(string city)
        {
            _logger.LogInformation("Fetching weather for city: {City}", city);

            // create cache key
            var cacheKey = $"weather_{city.ToLower()}";

            // check cache first
            if (_cache.TryGetValue(cacheKey, out CurrentWeatherDto? cachedWeather))
            {
                _logger.LogInformation("Weather data for {City} found in cache", city);
                cachedWeather!.FromCache = true;
                return cachedWeather;
            }

            // Not in cache, call external API
            try
            {
                _logger.LogDebug("Making API call to OpenWeatherMap for city: {City}", city);

                var url = $"weather?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "OpenWeatherMap API returned {StatusCode} for city: {City}",
                        response.StatusCode,
                        city);

                    if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        _logger.LogInformation("City {City} not found", city);
                        return null;
                    }

                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weatherData == null)
                {
                    _logger.LogError("Failed to deserialize weather data for city: {City}", city);
                    return null;
                }

                // Map to our DTO
                var result = MapToCurrentWeatherDto(weatherData);

                // cache for 10 minutes
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, result, cacheOptions);

                _logger.LogInformation(
                    "Successfully retrieved weather for {City}. Temperature: {Temperature}°C",
                    city,
                    result.Temperature);

                return result;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error fetching weather for city: {City}", city);
                throw new InvalidOperationException($"Failed to fetch weather data for {city}", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "JSON parsing error for city: {City}", city);
                throw new InvalidOperationException($"Failed to parse weather data for {city}", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching weather for city: {City}", city);
                throw;
            }
        }



        public async Task<CurrentWeatherDto?> GetCurrentWeatherByCoordinatesAsync(double lat, double lon)
        {
            
            _logger.LogInformation("Fetching weather for coordinates: Lat={Lat}, Lon={Lon}", lat, lon);

            var cacheKey = $"weather_{lat}_{lon}";

            if (_cache.TryGetValue(cacheKey, out CurrentWeatherDto? cachedWeather))
            {
                _logger.LogInformation("Weather data for coordinates found in cache");
                cachedWeather!.FromCache = true;
                return cachedWeather;
            }

            try
            {
                var url = $"weather?lat={lat}&lon={lon}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "OpenWeatherMap API returned {StatusCode} for coordinates",
                        response.StatusCode);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<OpenWeatherResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (weatherData == null)
                    return null;

                var result = MapToCurrentWeatherDto(weatherData);

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                _cache.Set(cacheKey, result, cacheOptions);

                _logger.LogInformation("Successfully retrieved weather for coordinates");

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather for coordinates");
                throw new InvalidOperationException("Failed to fetch weather data", ex);
            }
        }

        public async Task<ForecastDto?> GetForecastAsync(string city)
        {
            _logger.LogInformation("Fetching 5-day forecast for city: {City}", city);

            var cacheKey = $"forecast_{city.ToLower()}";

            if (_cache.TryGetValue(cacheKey, out ForecastDto? cachedForecast))
            {
                _logger.LogInformation("Forecast data for {City} found in cache", city);
                return cachedForecast;
            }

            try
            {
                var url = $"forecast?q={city}&appid={_apiKey}&units=metric";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning(
                        "OpenWeatherMap API returned {StatusCode} for forecast: {City}",
                        response.StatusCode,
                        city);
                    return null;
                }

                var content = await response.Content.ReadAsStringAsync();
                var forecastData = JsonSerializer.Deserialize<OpenWeatherForecastResponse>(
                    content,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (forecastData == null)
                    return null;

                var result = MapToForecastDto(forecastData);

                // Cache forecast for 30 minutes
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30));

                _cache.Set(cacheKey, result, cacheOptions);

                _logger.LogInformation(
                    "Successfully retrieved forecast for {City} with {Count} items",
                    city,
                    result.Items.Count);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching forecast for city: {City}", city);
                throw new InvalidOperationException($"Failed to fetch forecast for {city}", ex);
            }
        }
        private CurrentWeatherDto MapToCurrentWeatherDto(OpenWeatherResponse weather)
        {
            return new CurrentWeatherDto
            {
                City = weather.Name,
                Temperature = Math.Round(weather.Main.Temp, 1),
                FeelsLike = Math.Round(weather.Main.Feels_Like, 1),
                TempMin = Math.Round(weather.Main.Temp_Min, 1),
                TempMax = Math.Round(weather.Main.Temp_Max, 1),
                Humidity = weather.Main.Humidity,
                Pressure = weather.Main.Pressure,
                Description = weather.Weather.FirstOrDefault()?.Description ?? "N/A",
                MainCondition = weather.Weather.FirstOrDefault()?.Main ?? "N/A",
                WindSpeed = Math.Round(weather.Wind.Speed, 1),
                WindDirection = weather.Wind.Deg,
                Cloudiness = weather.Clouds.All,
                LastUpdated = DateTimeOffset.FromUnixTimeSeconds(weather.Dt).DateTime,
                Icon = weather.Weather.FirstOrDefault()?.Icon ?? "",
                FromCache = false
            };
        }

        private ForecastDto MapToForecastDto(OpenWeatherForecastResponse forecast)
        {
            return new ForecastDto
            {
                City = forecast.City.Name,
                Country = forecast.City.Country,
                Items = forecast.List.Select(item => new ForecastItemDto
                {
                    DateTime = DateTimeOffset.FromUnixTimeSeconds(item.Dt).DateTime,
                    Temperature = Math.Round(item.Main.Temp, 1),
                    FeelsLike = Math.Round(item.Main.Feels_Like, 1),
                    Description = item.Weather.FirstOrDefault()?.Description ?? "N/A",
                    MainCondition = item.Weather.FirstOrDefault()?.Main ?? "N/A",
                    WindSpeed = Math.Round(item.Wind.Speed, 1),
                    Humidity = item.Main.Humidity,
                    Icon = item.Weather.FirstOrDefault()?.Icon ?? ""
                }).ToList()
            };
        }
    }
}

