using BlazorDesktop.DataGeneration;
using BlazorDesktop.Infrastructure;

var context = new BlazorDesktopDbContextFactory().CreateDbContext(Array.Empty<string>());
var seedForecasts = WeatherGeneratorConfigurations.GenerateRandomForecasts(500);
context.AddRange(seedForecasts);
context.SaveChanges();