using BlazorDesktop.Application;
using BlazorDesktop.Application.Weather;
using BlazorDesktop.Infrastructure;
using Fluxor;
using Fluxor.Blazor.Web.ReduxDevTools;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace BlazorDesktop;

//Todo::make an application library for business logic ( will connect to infrastructure and domain )
//in the application we'll have our event orchestrators for the application state
//in this project we will create a couple ui components that use their own orchestrators and fluxor will update the ui
public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Configuration.AddUserSecrets(
			Assembly.GetExecutingAssembly(),
			optional: true);

		builder.Services.AddMauiBlazorWebView();
		builder.Services.AddFluxor(options =>
			options
				.ScanAssemblies(typeof(MauiProgram).Assembly)
				.UseReduxDevTools()
		);

		builder.Services.AddInfrastructure(builder.Configuration);
		builder.Services.AddApplication();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
		return builder.Build();
	}
}
