namespace weather_dashboard_api.DTOs
{
    public class CurrentWeatherDto
    {
        public string City { get; set; } = string.Empty;
        public double Temperature { get; set; }
        public double FeelsLike { get; set; }
        public double TempMin { get; set; }
        public double TempMax { get; set; }
        public int Humidity { get; set; }
        public int Pressure { get; set; }
        public string Description { get; set; } = string.Empty;
        public string MainCondition { get; set; } = string.Empty;
        public double WindSpeed { get; set; }
        public int WindDirection { get; set; }
        public int Cloudiness { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Icon { get; set; } = string.Empty;
        public bool FromCache { get; set; }
    }
}
