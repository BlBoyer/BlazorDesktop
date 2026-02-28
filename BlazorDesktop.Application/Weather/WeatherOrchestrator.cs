namespace BlazorDesktop.Application.Weather;

/// <summary>
/// This class mediates calls between the Presentation layer and the database. Each event requesting data gets handled by a `subscriber class`, which in dispatches an event to send data back of which handling is completed in the ui.
/// </summary>
public static class WeatherOrchestrator
{
	public record ForecastsRequestedEvent
	{
		public long TimestampTicks { get; } = DateTime.UtcNow.Ticks;
		public ForecastFilter? Filter { get; }
		public ForecastsRequestedEvent() { }
		public ForecastsRequestedEvent(ForecastFilter? filter = null) => Filter = filter;
	}
	public static event Action<ForecastsRequestedEvent>? ForecastsRequested;
	/// <summary>
	/// Dispatches a new <see cref="ForecastsRequestedEvent"/>. Handled in <see cref="WeatherSubscriber.HandleForecastsRequest(ForecastsRequestedEvent)"/>
	/// </summary>
	/// <param name="filter"></param>
	public static void RequestForecasts(ForecastFilter? filter = null)
	{
		ForecastsRequested?.Invoke(new(filter));
	}

	public record ForecastsEmittedEvent
	{
		public WeatherForecast[] Forecasts { get; }
		public ForecastsEmittedEvent(WeatherForecast[] forecasts) => Forecasts = forecasts;
	}
	/// <summary>
	/// Action dispatched with forecast payload. Handled in the ui state management '<see cref="BlazorDesktop.State.WeatherEffects.OnForecastsRecieved"/>'.
	/// </summary>
	public static event Action<ForecastsEmittedEvent>? ForecastsEmitted;
	/// <summary>
	/// Dispatches a new <see cref="ForecastsEmittedEvent"/>. Handled in '<see cref="BlazorDesktop.State.WeatherEffects.OnForecastsRecieved(ForecastsEmittedEvent)"/>
	/// </summary>
	/// <param name="payload"></param>
	public static void EmitForecasts(WeatherForecast[] payload) => ForecastsEmitted?.Invoke(new(payload));
}
