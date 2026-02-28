using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace BlazorDesktop.Infrastructure;

/// <summary>
/// This is a context factory for use with cli migrations so we don't need to run the whole app's configuration and services
/// </summary>
public class BlazorDesktopDbContextFactory : IDesignTimeDbContextFactory<BlazorDesktopContext>
{
	public BlazorDesktopContext CreateDbContext(string[] args)
	{
		var optionsBuilder = new DbContextOptionsBuilder<BlazorDesktopContext>();

		// Build a minimal configuration to read user secrets
		var configuration = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory()) // optional, adjusts path if needed
			.AddUserSecrets<BlazorDesktopDbContextFactory>()  // points to the assembly for secrets
			.Build();

		// Use SQLite for your demo
		optionsBuilder.UseSqlite(configuration.GetConnectionString("BlazorDesktopSqlServer"));

		return new BlazorDesktopContext(optionsBuilder.Options);
	}
}
