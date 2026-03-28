namespace StockApp.Models;

public class TransactionViewModel
{
    public string Id { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal PricePerUnit { get; set; }
    public DateTime Date { get; set; }
    public string Ticker { get; set; }
    public bool IsBuy { get; set; } = true;

}
