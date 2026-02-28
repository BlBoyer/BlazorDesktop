using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorDesktop.Infrastructure;

public static class ConfigureServices
{
	public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config) 
	{
		var connection = config.GetConnectionString("BlazorDesktopSqlServer");
		services.AddDbContext<BlazorDesktopContext>(options => options.UseSqlite(connection));
		return services;
	}
}
