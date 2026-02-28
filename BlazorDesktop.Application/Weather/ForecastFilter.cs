public record ForecastFilter
{
    public DateOnly? From { get; set; }
    public DateOnly? To { get; set; }
    public int? MinTemperature { get; set; }
    public int? MaxTemperature { get; set; }
}