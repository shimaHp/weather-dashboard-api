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

2. **Configure API Key**
```bash
git clone https://github.com/yourusername/weather-dashboard-api.git
cd weather-dashboard-api

3. **Get OpenWeatherMap API Key**
# Copy example configuration to actual config file
cp appsettings.example.json appsettings.json
# Windows:
# copy appsettings.example.json appsettings.json
Open appsettings.json and replace "YOUR_API_KEY_HERE" with your actual API key.

 4. **Run the application**
```bash
dotnet restore
dotnet run
```

5. **Open Swagger UI**
```
https://localhost:5001/swagger
```


### Error Handling Flow
```
1. Network Error (HttpRequestException)
   → Log error with context
   → Throw InvalidOperationException
   → Controller returns 503 Service Unavailable

2. Invalid City (404 from OpenWeatherMap)
   → Log warning
   → Return null
   → Controller returns 404 Not Found

3. JSON Parsing Error (JsonException)
   → Log error
   → Throw InvalidOperationException
   → Controller returns 503 Service Unavailable

4. Unexpected Error (Exception)
   → Log critical error
   → Controller returns 500 Internal Server Error

## 📊 Logging Examples

### Console Output
```
info: WeatherApi.Controllers.WeatherController[0]
      GET request received for weather in city: London
      
info: WeatherApi.Services.WeatherService[0]
      Fetching weather for city: London
      
dbug: WeatherApi.Services.WeatherService[0]
      Making API call to OpenWeatherMap for city: London
      
info: WeatherApi.Services.WeatherService[0]
      Successfully retrieved weather for London. Temperature: 15.2°C

# Second request (cached)
info: WeatherApi.Services.WeatherService[0]
      Weather data for London found in cache

# Error scenario
warn: WeatherApi.Services.WeatherService[0]
      OpenWeatherMap API returned NotFound for city: InvalidCity
      
warn: WeatherApi.Controllers.WeatherController[0]
      Weather data not found for city: InvalidCity

## 📚 What I Learned

### External API Integration
- Consuming third-party REST APIs
- Reading API documentation and mapping responses
- Handling API rate limits and quotas
- Using tools like json2csharp.com to generate models from JSON

### HttpClient Best Practices
- Why HttpClient factory pattern is crucial
- Avoiding socket exhaustion issues
- Proper HttpClient lifecycle management
- Configuring base addresses and default headers

### Caching Strategies
- When to cache and for how long
- Cache key design
- Cache invalidation strategies
- Balancing freshness vs performance

### Error Handling for External Dependencies
- Different error types (network, parsing, business logic)
- Graceful degradation
- Meaningful error messages for API consumers
- Logging errors with context for debugging

### Logging Best Practices
- Structured logging with parameters
- Appropriate log levels (Debug, Information, Warning, Error)
- Logging external API calls and their results
- Tracking cache hits/misses
