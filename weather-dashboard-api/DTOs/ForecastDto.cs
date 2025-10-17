namespace weather_dashboard_api.DTOs
{
    public class ForecastDto
    {
     
            public string City { get; set; } = string.Empty;
            public string Country { get; set; } = string.Empty;
            public List<ForecastItemDto> Items { get; set; } = new();
        }
    }

