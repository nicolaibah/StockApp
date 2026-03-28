namespace StockApp.Models;

public class TimeRange
{
    public TimeRange(string value)
    {
        Value = value;
    }
    public string Value { get; private set; }
    public static TimeRange OneDay => new("1d");
    public static TimeRange FiveDays => new("5d");
    public static TimeRange OneMonth => new("1m");
    public static TimeRange ThreeMonths => new("3m");
    public static TimeRange TenYears => new("1y");


    public DateTime GetStartDate() => Value switch
    {
        "1d" => DateTime.Today.AddDays(-1),
        "5d" => DateTime.Today.AddDays(-5),
        "1m" => DateTime.Today.AddMonths(-1),
        "3m" => DateTime.Today.AddMonths(-3),
        "1y" => DateTime.Today.AddYears(-1),
        _ => DateTime.Today.AddDays(-1)
    };
    public string GetInterval()
    {
        if(Value == "1d" || Value == "5d")
        {
            return "15m";
        }
        return "1d";
    }
}
