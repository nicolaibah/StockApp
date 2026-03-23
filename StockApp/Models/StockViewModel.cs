
namespace StockApp.Models;

public class StockViewModel
{
    public decimal Amount { get; set; }
    public decimal AveragePrice { get; set; }
    public string Ticker { get; set; }
    public decimal GetValueInvested()
    {
        return Amount * AveragePrice;
    }
    public decimal GetCurrentValue(Quote quote)
    {
        decimal currentValue = quote.CurrentPrice;
        return Amount * currentValue;
    }
    public decimal GetTotalGain(Quote quote)
    {
        return GetCurrentValue(quote) - GetValueInvested();
    }
    public decimal ValueInTargetCurrency { get; set;  }
}
