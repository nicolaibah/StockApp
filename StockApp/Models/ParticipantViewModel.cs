namespace StockApp.Models;

public class ParticipantViewModel
{
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Capital { get; set; }
    public List<TransactionViewModel> Transactions { get; set; } = [];
    public List<StockViewModel> Stocks { get; set; } = [];
    public List<ValuePoint> PortfolioDateValues { get; set; } = [];
}