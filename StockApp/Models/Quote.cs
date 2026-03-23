using System.Text.Json.Serialization;

namespace StockApp.Models;

public class Quote
{
    public string Ticker { get; set; }
    [JsonPropertyName("c")]
    public decimal CurrentPrice { get; set; }

    [JsonPropertyName("d")]
    public double Change { get; set; }

    [JsonPropertyName("dp")]
    public double PercentChange { get; set; }

    [JsonPropertyName("h")]
    public double HighOfTheDay { get; set; }

    [JsonPropertyName("l")]
    public double LowOfTheDay { get; set; }

    [JsonPropertyName("o")]
    public double OpenOfTheDay { get; set; }
    public string Currency { get; set; }
}
