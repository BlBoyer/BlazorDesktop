using Microsoft.EntityFrameworkCore;

namespace BlazorDesktop.Application.Weather;

public partial class WeatherSubscriber
{

	public async void ReceiveForecasts(WeatherOrchestrator.GetWeatherDataEvent @event) 
	{
		//get weather data from db
		var results = _context.WeatherForecast;
		var forecasts = await results.Select(x => new WeatherForecast(
			Date: x.Date,
			TemperatureC: x.TemperatureC,
			Summary: x.Summary
			)).ToArrayAsync();

		//dispatch event with payload
		WeatherOrchestrator.PublishForecasts(forecasts);
	}
}
