using weather_dashboard_api.DTOs;

namespace weather_dashboard_api.Services
{
    public interface IWeatherService
    {
        Task<CurrentWeatherDto?> GetCurrentWeatherAsync(string city);
        Task<CurrentWeatherDto?> GetCurrentWeatherByCoordinatesAsync(double lat, double lon);
        Task<ForecastDto?> GetForecastAsync(string city);
    }
}
