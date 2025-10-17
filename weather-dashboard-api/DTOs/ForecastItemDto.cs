namespace weather_dashboard_api.DTOs
{
    public class ForecastItemDto
        {
            public DateTime DateTime { get; set; }
            public double Temperature { get; set; }
            public double FeelsLike { get; set; }
            public string Description { get; set; } = string.Empty;
            public string MainCondition { get; set; } = string.Empty;
            public double WindSpeed { get; set; }
            public int Humidity { get; set; }
            public string Icon { get; set; } = string.Empty;
        }
    }

