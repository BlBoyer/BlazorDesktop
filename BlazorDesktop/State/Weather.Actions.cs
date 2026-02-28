using BlazorDesktop.Application.Weather;

namespace BlazorDesktop.State;

public static class WeatherActions
{
	public record LoadWeather { }
	public record SetForecasts(WeatherForecast[] payload);
	public record FilterWeather(ForecastFilter payload);
}
