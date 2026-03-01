
namespace BlazorDesktop.Testing;

using BlazorDesktop.Application.Weather;
using BlazorDesktop.Domain;
using BlazorDesktop.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using TUnit;
using static BlazorDesktop.Application.Weather.WeatherOrchestrator;

public class WeatherOrchestratorTests
{
	[Test]
	public async Task Subscriber_HandlesForecastsRequestAndEmitsForecastsCorrectly()
	{
		// Arrange
		bool handled = false;
		Application.Weather.WeatherForecast[]? resultForecasts = null;
		WeatherOrchestrator.ForecastsEmitted += (e) =>
		{
			handled = true;
			resultForecasts = e.Forecasts;
		};

		BlazorDesktopContext context = Data.InMemoryDb.CreateInMemoryDb();

		// Create a minimal subscriber instance
		var subscriber = new WeatherSubscriber(context);

		var payload = new Domain.WeatherForecast[]
		{
			new (){ Date = new DateOnly(), TemperatureC = 19, Summary = "Rainy" },
			new (){ Date = new DateOnly(), TemperatureC = 10, Summary = "Snowy" }
		};
		var applicationForecasts = payload.Select(x => new Application.Weather.WeatherForecast(x.Date, x.TemperatureC, x.Summary)).ToArray();

		context.WeatherForecast.AddRange(payload);
		await context.SaveChangesAsync();

		var requestEvent = new ForecastsRequestedEvent();

		// Act
		//WeatherOrchestrator.RequestForecasts(); //write new test for filtered request
		subscriber.HandleForecastsRequest(requestEvent);

		// Assert
		using (Assert.Multiple()) 
		{
			await Assert.That(handled, "Subscriber should handle the emitted event").IsTrue();
			await Assert.That(resultForecasts).IsNotNull();
			await Assert.That(resultForecasts).Count().IsEqualTo(12);	//seed data plus test data
			await Assert.That(resultForecasts).Contains(applicationForecasts[0]);
		}
	}
}
