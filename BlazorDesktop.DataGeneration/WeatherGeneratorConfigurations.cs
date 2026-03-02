using Bogus;
using Bogus.DataSets;

namespace BlazorDesktop.DataGeneration;

public static class WeatherGeneratorConfigurations
{
	public static Application.Weather.WeatherForecast NewForecastDto(Action<Application.Weather.WeatherForecast>? options = null) 
	{
		var bogus = new Faker<Application.Weather.WeatherForecast>()
			.RuleFor(x => x.Date, f => new Date().FutureDateOnly())
			.RuleFor(x => x.TemperatureC, f => f.Random.Int(-50,50))
			.RuleFor(x => x.Summary, f => new Lorem().Paragraph());
		
		var forecast = bogus.Generate();
		options?.Invoke(forecast);
		return forecast;
	}

	public static Domain.WeatherForecast NewForecast(Action<Domain.WeatherForecast>? options = null)
	{
		var bogus = new Faker<Domain.WeatherForecast>()
			.RuleFor(x => x.Date, f => new Date().FutureDateOnly())
			.RuleFor(x => x.TemperatureC, f => f.Random.Int(-50, 50))
			.RuleFor(x => x.Summary, f => new Lorem().Paragraph());

		var forecast = bogus.Generate();
		options?.Invoke(forecast);
		return forecast;
	}

	/// <summary>
	/// Generate an array of weather forecast DTOs.
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	public static Application.Weather.WeatherForecast[] GenerateRandomForecastDTOs(int n) 
	{
		List<Application.Weather.WeatherForecast> collection = new();
		for (int i = 0; i < n; i++) 
		{
			collection.Add(NewForecastDto());
		}
		return collection.ToArray();
	}

	/// <summary>
	/// Generate an array of weather forecast entities.
	/// </summary>
	/// <param name="n"></param>
	/// <returns></returns>
	public static Domain.WeatherForecast[] GenerateRandomForecasts(int n)
	{
		List<Domain.WeatherForecast> collection = new();
		for (int i = 0; i < n; i++)
		{
			collection.Add(NewForecast());
		}
		return collection.ToArray();
	}
}
