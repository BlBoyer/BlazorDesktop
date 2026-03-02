using BlazorDesktop.Application.Weather;
using BlazorDesktop.Infrastructure;
using static BlazorDesktop.Application.Weather.WeatherOrchestrator;

namespace BlazorDesktop.Testing;

public class WeatherOrchestratorTests
{
	[Test]
	public async Task Subscriber_HandlesForecastsRequestAndEmitsForecastsCorrectly()
	{
		// Arrange
		bool handled = false;
		Application.Weather.WeatherForecast[]? emittedForecasts = null;
		WeatherOrchestrator.ForecastsEmitted += (e) =>
		{
			handled = true;
			emittedForecasts = e.Forecasts;
		};

		BlazorDesktopContext context = Data.InMemoryDb.CreateInMemoryDb();

		var subscriber = new WeatherSubscriber(context);

		var payload = DataGeneration.WeatherGeneratorConfigurations.GenerateRandomForecasts(5);

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
			await Assert.That(emittedForecasts).IsNotNull();
			await Assert.That(emittedForecasts).Count().IsEqualTo(5);	//seed data plus test data
			await Assert.That(emittedForecasts).Contains(applicationForecasts[0]);
		}
	}

	//ToDo::create a theory test for each filter property
	[Test]
	public async Task Subscriber_HandlesFilteredForecastsRequestAndEmitsFilteredForecastsCorrectly()
	{
		// Arrange
		
		// Set effects for checking results against
		bool handled = false;
		Application.Weather.WeatherForecast[]? emittedForecasts = null;
		WeatherOrchestrator.ForecastsEmitted += (e) =>
		{
			handled = true;
			emittedForecasts = e.Forecasts;
		};

		// Seed database with fresh entities and inject context
		BlazorDesktopContext context = Data.InMemoryDb.CreateInMemoryDb();
		var subscriber = new WeatherSubscriber(context);
		var payload = DataGeneration.WeatherGeneratorConfigurations.GenerateRandomForecasts(20);
		var applicationForecasts = payload.Select(x => new Application.Weather.WeatherForecast(x.Date, x.TemperatureC, x.Summary)).ToArray();
		context.WeatherForecast.AddRange(payload);
		await context.SaveChangesAsync();

		// Setup trial event
		var filter = new ForecastFilter()
		{
			MaxTemperature = 30
		};
		var requestEvent = new ForecastsRequestedEvent(filter);

		// Act
		subscriber.HandleForecastsRequest(requestEvent);

		// Assert
		using (Assert.Multiple())
		{
			await Assert.That(handled, "Subscriber should handle the emitted event").IsTrue();
			await Assert.That(emittedForecasts).IsNotNull();
			await Assert.That(emittedForecasts).All(x => x.TemperatureC <= filter.MaxTemperature);
		}
	}

	//ToDo::test database entity creation and updates
}
