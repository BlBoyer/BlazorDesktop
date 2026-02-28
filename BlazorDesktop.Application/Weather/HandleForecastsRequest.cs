using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BlazorDesktop.Application.Weather;

public partial class WeatherSubscriber
{

	public async void HandleForecastsRequest(WeatherOrchestrator.ForecastsRequestedEvent @event) 
	{
		//we can use an out method for this also...
		Domain.WeatherForecast[] results;
		//get weather data from db
		if (@event.Filter is not null)
		{
			//apply filter to db query
			results = await _context.WeatherForecast.Where(ForecastsMatchFilter(@event.Filter)).ToArrayAsync();
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
		WeatherOrchestrator.EmitForecasts(forecasts);
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

