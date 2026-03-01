using BlazorDesktop.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace BlazorDesktop.Testing.Data;

public static class InMemoryDb
{
	public static BlazorDesktopContext CreateInMemoryDb()
	{
		var options = new DbContextOptionsBuilder<BlazorDesktopContext>()
			.UseSqlite("DataSource=:memory:")
			.Options;

		var context = new BlazorDesktopContext(options);
		context.Database.OpenConnection();
		context.Database.EnsureCreated();

		return context;
	}

}