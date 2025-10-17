using Microsoft.Extensions.Caching.Memory;
using weather_dashboard_api.DTOs;

namespace weather_dashboard_api.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey;
        private readonly string _baseUrl = "https://api.openweathermap.org/data/2.5/";

        public Task<CurrentWeatherDto?> GetCurrentWeatherAsync(string city)
        {
            throw new NotImplementedException();
        }

        public Task<CurrentWeatherDto?> GetCurrentWeatherByCoordinatesAsync(double lat, double lon)
        {
            throw new NotImplementedException();
        }

        public Task<ForecastDto?> GetForecastAsync(string city)
        {
            throw new NotImplementedException();
        }
    }
}
