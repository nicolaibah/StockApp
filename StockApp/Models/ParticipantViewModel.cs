using StockTrackingApi.Models.StockModels;
using System.Transactions;

namespace StockApp.Models;

public class ParticipantViewModel
{
    public string Email { get; set; }
    public string Name { get; set; }
    public double Capital { get; set; }
    public List<TransactionViewModel> Transactions { get; set; } = [];
    public List<StockViewModel> Stocks { get; set; }
  
}