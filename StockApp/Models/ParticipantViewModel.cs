using StockTrackingApi.Models.StockModels;
using System.Transactions;
using static StockApp.Pages.Game;

namespace StockApp.Models;

public class ParticipantViewModel
{
    public string Email { get; set; }
    public string Name { get; set; }
    public double Capital { get; set; }
    public List<TransactionViewModel> Transactions { get; set; } = [];
    public List<StockViewModel> Stocks { get; set; }
    
    public List<ValuePoint> ValuePoints { get; set; }
}