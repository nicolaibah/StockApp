using StockLib;
using System.Linq;

namespace StockApp.Models;

public class StockViewModel
{
    public TimeRange TimeRange { get; set; } = TimeRange.FiveDays;
    public decimal Amount { get; set; }
    public decimal AveragePrice { get; set; }
    public decimal CurrentExchangeRate { get; set; } = 1;


    public string Ticker { get; set; } = string.Empty;

    // The current total value of this position (Amount * Latest Price)
    public decimal ValueInTargetCurrency { get; set; }

    // Historical points fetched from the API
    public List<ValuePoint> HistoricalValues { get; set; } = new();

    /// <summary>
    /// Calculates the initial cost basis (what you paid for the shares).
    /// </summary>
    public decimal GetValueInvested()
    {
        return Amount * AveragePrice;
    }

    /// <summary>
    /// Returns the total gain in absolute currency (e.g., +500 kr) 
    /// based on your purchase price.
    /// </summary>
    public decimal GetTotalGainAmount()
    {
        return ValueInTargetCurrency - GetValueInvested();
    }

    /// <summary>
    /// Calculates the percentage change over the selected TimeRange (1d, 5d, 1m, etc.).
    /// This compares CURRENT market price to HISTORICAL market price.
    /// </summary>
    public decimal GetPercentageGain()
    {
        ValuePoint historicalPoint = GetValuePoint();

        // Safety check
        if (historicalPoint == null || historicalPoint.Value <= 0 || Amount <= 0)
            return 0;

        // NORMALIZATION FIX:
        // Convert the historical unit price (e.g. USD) to the target currency (e.g. DKK)
        decimal historicalPriceInTargetCurrency = historicalPoint.Value * CurrentExchangeRate;

        // Get the current unit price in target currency
        decimal currentUnitPriceInTargetCurrency = ValueInTargetCurrency / Amount;

        // Formula: ((Current - Historical) / Historical) * 100
        return ((currentUnitPriceInTargetCurrency - historicalPriceInTargetCurrency) / historicalPriceInTargetCurrency) * 100;
    }
    /// <summary>
    /// Logic to find the correct historical "baseline" point based on the selected TimeRange.
    /// </summary>
    private ValuePoint? GetValuePoint()
    {
        if (HistoricalValues == null || !HistoricalValues.Any()) return null;

        // Ensure chronological order (Oldest -> Newest)
        var sorted = HistoricalValues.OrderBy(x => x.Date).ToList();
        int count = sorted.Count;

        return TimeRange.Value switch
        {
            // For 1 day: Try to get the previous closing price (second to last)
            "1d" => count >= 2 ? sorted[^2] : sorted[0],

            // For 5 days: Use the oldest point in the 5-day fetch as the baseline
            "5d" => sorted[0],

            // Calendar-based logic for longer windows
            "1m" => GetByCalendar(sorted, DateTime.Now.AddMonths(-1)),
            "3m" => GetByCalendar(sorted, DateTime.Now.AddMonths(-3)),
            "1y" => GetByCalendar(sorted, DateTime.Now.AddYears(-1)),

            // Fallback to the oldest available data point
            _ => sorted[0]
        };
    }

    private ValuePoint? GetByCalendar(List<ValuePoint> sorted, DateTime targetDate)
    {
        // Find the latest trading day that is on or before our target calendar date
        // (Excludes today to ensure we aren't comparing today to today)
        return sorted
            .Where(x => x.Date.Date < DateTime.Now.Date)
            .LastOrDefault(x => x.Date.Date <= targetDate.Date)
            ?? sorted[0];
    }
    public decimal GetCurrentValue(Quote quote)
    {
        if (quote == null) return 0;
        return Amount * quote.CurrentPrice;
    }
}