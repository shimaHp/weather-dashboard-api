namespace weather_dashboard_api.Models
{
    // Forecast response (5-day forecast)
    public class OpenWeatherForecastResponse
    {
        public ForecastItem[] List { get; set; } = Array.Empty<ForecastItem>();
        public CityData City { get; set; } = new();
    }
}
