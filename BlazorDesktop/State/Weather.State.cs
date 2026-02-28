using BlazorDesktop.Application.Weather;
using Fluxor;
using static BlazorDesktop.Application.Weather.WeatherOrchestrator;

namespace BlazorDesktop.State;

public record WeatherState
{
	public WeatherForecast[]? Forecasts { get; init; }
	public string[]? Errors { get; init; }
}


public class WeatherFeature : Feature<WeatherState>
{
	public override string GetName() => "Weather";
	protected override WeatherState GetInitialState()
	{
		return new WeatherState
		{
			Forecasts = Array.Empty<WeatherForecast>(),
			Errors = Array.Empty<string>()
		};
	}
}


public static class Reducers
{
	[ReducerMethod]
	public static WeatherState ForecastsReceived(WeatherState state, WeatherActions.SetForecasts action)
	{
		return state with
		{
			Forecasts = action.payload
		};
	}
}

/*Effects*/

public class WeatherEffects
{
	private readonly Fluxor.IDispatcher _dispatcher;
	private readonly IState<WeatherState> _state;
	public WeatherEffects(Fluxor.IDispatcher dispatcher, IState<WeatherState> state)
	{
		_dispatcher = dispatcher;
		_state = state;

		// Subscribe to orchestrator events
		WeatherOrchestrator.PublishForecastsRequested += OnWeatherReceived;
	}

	private void OnWeatherReceived(PublishForecastsEvent @event)
	{
		// Dispatch to Fluxor state store
		_dispatcher.Dispatch(new WeatherActions.SetForecasts(@event.Forecasts));
	}

	[EffectMethod(typeof(WeatherActions.LoadWeather))]
	public async Task EnsureWeatherLoaded(Fluxor.IDispatcher dispatcher)
	{
		if (_state.Value.Forecasts?.Length == 0)
		{
			WeatherOrchestrator.DispatchGetWeather();
			//set state isLoading to true when needed
		}
	}

	//we're keeping this for a parameterized call
	//[EffectMethod(typeof(WeatherActions.FilterWeather))]
	//public async Task GetWeatherForecasts(Fluxor.IDispatcher dispatcher)
	//{
	//	WeatherOrchestrator.DispatchGetWeather();
	//}
}