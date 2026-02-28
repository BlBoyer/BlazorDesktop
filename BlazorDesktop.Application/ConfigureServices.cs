using BlazorDesktop.Application.Weather;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDesktop.Application;

public static class ConfigureServices
{
	public static IServiceCollection AddApplication(this IServiceCollection services)
	{
		services.AddSingleton<Weather.WeatherSubscriber>();
		//call once to instantiate service
		var _ = services.BuildServiceProvider().GetRequiredService<WeatherSubscriber>();
		return services;
	}
}
