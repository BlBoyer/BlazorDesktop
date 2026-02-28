namespace BlazorDesktop.Application.Weather;

public static class WeatherOrchestrator
{
	public record GetWeatherDataEvent
	{
		public long TimestampTicks { get; } = DateTime.UtcNow.Ticks;
	}
	public static event Action<GetWeatherDataEvent>? GetWeatherData;
	public static void DispatchGetWeather()
	{
		GetWeatherData?.Invoke(new());
	}

	public record PublishForecastsEvent
	{
		public WeatherForecast[] Forecasts { get; }
		public PublishForecastsEvent(WeatherForecast[] forecasts) => Forecasts = forecasts;
	}
	public static event Action<PublishForecastsEvent>? PublishForecastsRequested;
	public static void PublishForecasts(WeatherForecast[] payload) => PublishForecastsRequested?.Invoke(new(payload));
}
