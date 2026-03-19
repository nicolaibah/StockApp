namespace StockApp.Models;

public class TransactionViewModel
{
    public string Id { get; set; } = string.Empty;
    public double Amount { get; set; }
    public double PricePerUnit { get; set; }
    public DateTime Date { get; set; }
    public string Ticker { get; set; }
    public bool IsBuy { get; set; } = true;

}
