namespace weather_dashboard_api.Models
{
    public class ForecastItem
    {
        public long Dt { get; set; }
        public MainData Main { get; set; } = new();
        public WeatherDescription[] Weather { get; set; } = Array.Empty<WeatherDescription>();
        public WindData Wind { get; set; } = new();
        public CloudData Clouds { get; set; } = new();
        public string Dt_Txt { get; set; } = string.Empty;
    }
}
