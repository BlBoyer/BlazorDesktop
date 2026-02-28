using BlazorDesktop.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BlazorDesktop.Infrastructure.ModelConfiguration;

internal class WeatherForecastConfig : IEntityTypeConfiguration<WeatherForecast>
{
	public void Configure(EntityTypeBuilder<WeatherForecast> builder)
	{
		builder.ToTable("WeatherForecast");

		builder.HasKey(x => x.Id);

		builder.Property(x => x.Id)
			   .ValueGeneratedOnAdd();

		builder.Property(x => x.Date)
			   .IsRequired()
			   .HasConversion(
				   v => v.ToString("yyyy-MM-dd"),
				   v => DateOnly.Parse(v))
			   .HasColumnType("TEXT");

		builder.HasIndex(x => x.Date)
			   .IsUnique();

		builder.Property(x => x.TemperatureC)
			   .IsRequired();

		builder.ToTable(t =>
			t.HasCheckConstraint(
				"CK_WeatherForecast_TemperatureC",
				"TemperatureC BETWEEN -100 AND 100"));

		builder.Property(x => x.Summary)
			   .HasMaxLength(2048)
			   .IsRequired(false);
	}
}
