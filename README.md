# Weather Dashboard API

A RESTful API that provides current weather and forecast data by integrating with OpenWeatherMap API. Features intelligent caching, robust error handling, and comprehensive logging.

## 🎯 Features

### Core Functionality
- **Current Weather**: Get real-time weather data by city name or coordinates
- **5-Day Forecast**: Retrieve weather predictions with 3-hour intervals
- **Intelligent Caching**: Reduces API calls with 10-minute cache for current weather and 30-minute cache for forecasts
- **Error Handling**: Graceful handling of network failures, invalid requests, and API errors

### Technical Highlights
- **HttpClient Factory Pattern**: Proper HttpClient lifecycle management
- **Memory Caching**: Performance optimization with IMemoryCache
- **Structured Logging**: Comprehensive logging with ILogger
- **Clean DTOs**: Separation between external API models and internal response models
- **External API Integration**: Real-world third-party API consumption

## 🛠️ Technologies

- **ASP.NET Core 8** - Web API framework
- **HttpClient Factory** - HTTP client management
- **IMemoryCache** - In-memory caching
- **ILogger** - Structured logging
- **OpenWeatherMap API** - Weather data provider
- **C# 12** - Programming language

## 📋 API Endpoints

| Method | Endpoint | Description | Parameters |
|--------|----------|-------------|------------|
| GET | `/api/weather/current/{city}` | Get current weather by city | `city` (string) |
| GET | `/api/weather/current` | Get current weather by coordinates | `lat` (double), `lon` (double) |
| GET | `/api/weather/forecast/{city}` | Get 5-day forecast | `city` (string) |

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK or later
- OpenWeatherMap API key (free)
- Visual Studio 2022 / VS Code / Rider
- Postman or similar API testing tool (optional)

### Configuration Files
- `appsettings.example.json` contains the structure for required configuration.
- **Do not commit `appsettings.json`** with your actual API key.
- Users should copy the example file and insert their own API key.

### Installation

1. **Get OpenWeatherMap API Key**
   - Go to: https://openweathermap.org/api
   - Sign up for a free account
   - Navigate to API Keys section
   - Copy your API key (takes 10-15 minutes to activate)

2. **Clone the repository**
```bash
git clone https://github.com/yourusername/weather-dashboard-api.git
cd weather-dashboard-api
