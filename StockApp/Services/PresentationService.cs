using StockApp.Models;
using StockTrackingApi.Services;

namespace StockApp.Services;

public class PresentationService
{
    private readonly IExchangeRateService _exchangeService;
    private readonly IGameService _gameService;
    private List<ParticipantViewModel> _participants = [];
    public string TargetCurrency { get; set; } = "DKK";
    private decimal GameCapital { get; set; } = 0;
    public PresentationService(IExchangeRateService exchangeService, IGameService gameService)
    {
        _exchangeService = exchangeService;
        _gameService = gameService;
    }
    public async Task Init(List<ParticipantViewModel> participants, decimal gameCapital)
    {
        GameCapital = gameCapital;
        _participants = participants;
    }
    public async Task LoadHistoricalData(List<DateTime> dates)
    {
        foreach (var p in _participants)
        {
            p.ValuePoints = await GetValueForDateTimes(dates, TargetCurrency, GameCapital, p.Transactions);
        }
    }
    public async Task<decimal> GetValueInTargetCurrency(StockViewModel stock, string targetCurrency = "DKK")
    {
        Quote q = await _gameService.GetQuote(stock.Ticker);

        decimal currentValue = stock.GetCurrentValue(q);

        decimal finalValue = currentValue;

        if (q.Currency != targetCurrency)
        {
            var exchangeRate = await _exchangeService.ExchangeRate(q.Currency, targetCurrency);
            finalValue = currentValue * exchangeRate;
        }
        return finalValue;
    }
    public async Task<List<ValuePoint>> GetValueForDateTimes(List<DateTime> dates, string targetCurrency = "DKK", decimal initialCash = 0, IEnumerable<TransactionViewModel>? transactions = null)
    {
        var result = new List<ValuePoint>();
        var transactionList = transactions?.ToList() ?? new List<TransactionViewModel>();

        // Get all unique tickers from transactions
        var tickers = transactionList.Select(t => t.Ticker).Distinct().ToList();

        // Cache history and quotes for all stocks to avoid repeated calls
        var stockHistoryCache = new Dictionary<string, List<ValuePoint>>();
        var stockExchangeRateCache = new Dictionary<string, decimal>();

        foreach (var ticker in tickers)
        {
            var history = await _gameService.GetHistory(ticker, dates.Min());
            stockHistoryCache[ticker] = history;
            var quote = await _gameService.GetQuote(ticker);
            var exchangeRate = await GetExchangeRateIfNeeded(quote.Currency, targetCurrency);
            stockExchangeRateCache[ticker] = exchangeRate;
        }

        foreach (var date in dates)
        {
            decimal portfolioValue = 0;

            // Calculate holdings for each stock at this date from transactions
            var holdingsByTicker = new Dictionary<string, decimal>();
            foreach (var transaction in transactionList.Where(t => t.Date.Date <= date.Date))
            {
                if (!holdingsByTicker.ContainsKey(transaction.Ticker))
                    holdingsByTicker[transaction.Ticker] = 0;

                decimal amount = transaction.IsBuy ? (decimal)transaction.Amount : -(decimal)transaction.Amount;
                holdingsByTicker[transaction.Ticker] += amount;
            }

            // Calculate stock values for this date using holdings and cached data
            foreach (var (ticker, amount) in holdingsByTicker)
            {
                if (amount > 0) // Only value if we own shares
                {
                    var history = stockHistoryCache[ticker];
                    var exchangeRate = stockExchangeRateCache[ticker];

                    // Try to get exact date match, otherwise use the most recent data point before this date
                    var valuePoint = history.FirstOrDefault(h => h.Date.Date == date.Date)
                        ?? history.Where(h => h.Date.Date <= date.Date).OrderByDescending(h => h.Date).FirstOrDefault();

                    if (valuePoint != null)
                    {
                        portfolioValue += (valuePoint.Value * exchangeRate) * amount;
                    }
                }
            }

            // Calculate remaining cash at this date
            decimal remainingCash = CalculateRemainingCash(date, initialCash, transactionList, stockExchangeRateCache);
            portfolioValue += remainingCash;

            result.Add(new ValuePoint { Date = date, Value = portfolioValue });
        }

        return result;
    }

    private async Task<decimal> GetExchangeRateIfNeeded(string fromCurrency, string targetCurrency)
    {
        if (fromCurrency == targetCurrency)
            return 1;

        return await _exchangeService.ExchangeRate(fromCurrency, targetCurrency);
    }
    private decimal CalculateRemainingCash(DateTime date, decimal initialCash, List<TransactionViewModel> transactions, Dictionary<string, decimal> exchangeRateCache)
    {
        decimal cashSpent = 0;

        foreach (var transaction in transactions.Where(t => t.Date.Date <= date.Date))
        {
            decimal transactionAmount = (decimal)(transaction.Amount * transaction.PricePerUnit);

            if (transaction.IsBuy)
            {
                cashSpent += transactionAmount;
            }
            else
            {
                cashSpent -= transactionAmount;
            }
        }

        return initialCash - cashSpent;
    }
}
