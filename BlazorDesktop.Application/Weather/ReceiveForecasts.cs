using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlazorDesktop.Application.Weather;

public partial class WeatherSubscriber
{

	public async void ReceiveForecasts(WeatherOrchestrator.GetWeatherDataEvent @event) 
	{
		//we can use an out method for this also...
		Domain.WeatherForecast[] results;
		//get weather data from db
		if (@event.Filter is not null)
		{
			//apply filter to db query
			// use predicate to filter results
			results = await _context.WeatherForecast.Where(ForecastsMatchFilter(@event.Filter)).ToArrayAsync();
			// results = _context.WeatherForecast.Where(x =>
			// 	(@event.Filter.From is null || x.Date >= @event.Filter.From) &&
			// 	(@event.Filter.To is null || x.Date <= @event.Filter.To) &&
			// 	(@event.Filter.MinTemperature is null || x.TemperatureC >= @event.Filter.MinTemperature) &&
			// 	(@event.Filter.MaxTemperature is null || x.TemperatureC <= @event.Filter.MaxTemperature)
			// );
		}
		else
		{			
			results = await _context.WeatherForecast.ToArrayAsync();
		}

		WeatherForecast[] forecasts =  results.Select(x => new WeatherForecast(
			Date: x.Date,
			TemperatureC: x.TemperatureC,
			Summary: x.Summary
			)).ToArray();

		//dispatch event with payload
		WeatherOrchestrator.PublishForecasts(forecasts);
	}

	//create predicate for query filter
	private static Expression<Func<Domain.WeatherForecast, bool>> ForecastsMatchFilter(ForecastFilter filter)
	{
		return x =>
			(filter.From == null || x.Date >= filter.From) &&
			(filter.To == null || x.Date <= filter.To) &&
			(filter.MinTemperature == null || x.TemperatureC >= filter.MinTemperature) &&
			(filter.MaxTemperature == null || x.TemperatureC <= filter.MaxTemperature);
	}
}

