using BlazorDesktop.Infrastructure;

namespace BlazorDesktop.Application.Weather;

public partial class WeatherSubscriber
{
	//inject db context
	private readonly BlazorDesktopContext _context;
	public WeatherSubscriber(BlazorDesktopContext context)
	{
		_context = context;
		WeatherOrchestrator.ForecastsRequested += HandleForecastsRequest;
	}

}