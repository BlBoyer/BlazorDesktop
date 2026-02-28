using BlazorDesktop.Domain;
using Microsoft.EntityFrameworkCore;

namespace BlazorDesktop.Infrastructure;

public class BlazorDesktopContext : DbContext
{
	public BlazorDesktopContext(DbContextOptions options) : base(options) 
	{
		Console.WriteLine("SQLite connection string: " + Database.GetDbConnection().ConnectionString);
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	=> optionsBuilder
		.UseSeeding((context, _) =>
		{
			context.Set<WeatherForecast>().AddRange([
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-02-27"),
				TemperatureC = 5,
				Summary = "Partly cloudy"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-02-28"),
				TemperatureC = 7,
				Summary = "Sunny"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-01"),
				TemperatureC = 10,
				Summary = "Rain showers"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-02"),
				TemperatureC = 12,
				Summary = "Cloudy"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-03"),
				TemperatureC = 15,
				Summary = "Sunny"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-04"),
				TemperatureC = 8,
				Summary = "Foggy"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-05"),
				TemperatureC = 3,
				Summary = "Snow flurries"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-06"),
				TemperatureC = 6,
				Summary = "Windy"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-07"),
				TemperatureC = 9,
				Summary = "Partly sunny"
			},
			new WeatherForecast
			{
				Id = Guid.NewGuid(),
				Date = DateOnly.Parse("2026-03-08"),
				TemperatureC = 11,
				Summary = "Showers"
			},
			]);
			context.SaveChanges();
		});

	public DbSet<WeatherForecast> WeatherForecast { get; set; }
}
