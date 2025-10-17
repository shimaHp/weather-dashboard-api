namespace weather_dashboard_api.Models
{
    public class OpenWeatherResponse
    {
        public MainData Main { get; set; } = new();
        public WeatherDescription[] Weather { get; set; } = Array.Empty<WeatherDescription>();
        public WindData Wind { get; set; } = new();
        public CloudData Clouds { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public long Dt { get; set; }
    }
}
