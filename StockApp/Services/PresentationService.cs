using StockApp.Models;
using StockLib;
using StockTrackingApi.Services;
using System.Collections.Concurrent;

namespace StockApp.Services;

public class PresentationService
{
    private readonly IExchangeRateService _exchangeService;
    private readonly IGameService _gameService;
    private List<ParticipantViewModel> _participants = new();

    // Cache entry with timestamp so we can expire entries when needed
    private class CacheEntry
    {
        public List<ValuePoint> Data { get; set; } = new();
        public DateTime FetchedAtUtc { get; set; } = DateTime.UtcNow;
    }

    private readonly ConcurrentDictionary<string, CacheEntry> _historicalDataCache = new();
    private TimeSpan _cacheTtl = TimeSpan.FromMinutes(30);
    private TimeRange _currentTimeRange = TimeRange.FiveDays;

    public string TargetCurrency { get; set; } = "DKK";
    private decimal GameCapital { get; set; } = 0;

    public PresentationService(IExchangeRateService exchangeService, IGameService gameService)
    {
        _exchangeService = exchangeService;
        _gameService = gameService;
    }

    // Backwards-compatible Init overloads. Default to 5d if no range provided.
    public Task Init(List<ParticipantViewModel> participants, decimal gameCapital)
        => Init(participants, gameCapital, TimeRange.FiveDays);

    public async Task Init(List<ParticipantViewModel> participants, decimal gameCapital, TimeRange t)
    {
        GameCapital = gameCapital;
        _participants = participants ?? new List<ParticipantViewModel>();
        _currentTimeRange = t ?? TimeRange.FiveDays;

        await SetStockValues(_currentTimeRange);

        // Remove debug code that can throw when participant or stocks are empty
    }

    private async Task<decimal> GetValueInTargetCurrency(StockViewModel stock, string targetCurrency = "DKK")
    {
        if (stock == null || string.IsNullOrEmpty(stock.Ticker))
            return 0;

        Quote q = await _gameService.GetQuote(stock.Ticker);
        if (q == null) return 0;

        decimal currentValue = stock.GetCurrentValue(q);
        decimal finalValue = currentValue;

        stock.CurrentExchangeRate = 1;

        if (!string.Equals(q.Currency, targetCurrency, StringComparison.OrdinalIgnoreCase))
        {
            var exchangeRate = await _exchangeService.ExchangeRate(q.Currency, targetCurrency);
            stock.CurrentExchangeRate = exchangeRate;
            finalValue = currentValue * exchangeRate;
        }

        return finalValue;
    }

    private decimal CalculateRemainingCash(DateTime date, List<TransactionViewModel> transactions)
    {
        decimal cashSpent = 0;

        foreach (var transaction in transactions?.Where(t => t.Date.Date <= date.Date) ?? Enumerable.Empty<TransactionViewModel>())
        {
            decimal transactionAmount = (decimal)(transaction.Amount * (double)transaction.PricePerUnit);

            if (transaction.IsBuy)
                cashSpent += transactionAmount;
            else
                cashSpent -= transactionAmount;
        }

        return GameCapital - cashSpent;
    }

    private async Task SetStockValues(TimeRange t)
    {
        // Ensure historical data is fetched and attached to stock models
        await SetTickerHistory(t);

        try
        {
            // Set current values and exchange rates for each stock before calculating player time series
            var allStocks = _participants.SelectMany(x => x.Stocks ?? new List<StockViewModel>());
            foreach (var stock in allStocks)
            {
                stock.TimeRange = t;
                stock.ValueInTargetCurrency = await GetValueInTargetCurrency(stock, TargetCurrency);
            }

            // Now that current exchange rates and values are available on stock view models,
            // calculate player portfolio time series so historical points can use the correct FX.
            await SetPlayerValues(t);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error setting up values: {ex.Message}");
        }
    }

    private async Task SetPlayerValues(TimeRange t)
    {
        var dates = GenerateDateRangeForTimespan(t);
        foreach (var player in _participants)
        {
            player.PortfolioDateValues = await GetValueForDateTimes(player, dates, t);
        }
    }

    private List<DateTime> GenerateDateRangeForTimespan(TimeRange t)
    {
        int daysBack = t?.Value switch { "1d" => 1, "5d" => 5, "1m" => 30, "3m" => 90, "1y" => 365, _ => 5 };
        IEnumerable<DateTime> dates = Enumerable.Range(0, daysBack + 1).Select(i => DateTime.Now.AddDays(-i)).OrderBy(d => d);

        // Skip weekends for multi-day ranges (markets are closed on Saturday/Sunday)
        if (t?.Value != "1d")
            dates = dates.Where(d => d.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday);

        return dates.ToList();
    }

    public async Task SetTickerHistory(TimeRange t)
    {
        if (t == null) t = TimeRange.FiveDays;
        _currentTimeRange = t;

        var allTickers = _participants
            .SelectMany(p => p.Transactions?.Select(tr => tr.Ticker) ?? Enumerable.Empty<string>())
            .Where(s => !string.IsNullOrEmpty(s))
            .Distinct()
            .ToList();

        foreach (var ticker in allTickers)
        {
            string key = TickerToCacheKey(ticker, t);

            if (_historicalDataCache.TryGetValue(key, out var entry))
            {
                // If cache entry exists and is fresh, skip
                if ((DateTime.UtcNow - entry.FetchedAtUtc) < _cacheTtl)
                    continue;
            }

            try
            {
                var historicalValues = await _gameService.GetHistory(ticker, t);
                var cacheEntry = new CacheEntry { Data = historicalValues ?? new List<ValuePoint>(), FetchedAtUtc = DateTime.UtcNow };
                _historicalDataCache[key] = cacheEntry;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching history for {ticker}: {ex.Message}");
                _historicalDataCache[key] = new CacheEntry { Data = new List<ValuePoint>(), FetchedAtUtc = DateTime.UtcNow };
            }
        }

        // Attach cached history to stock view models
        foreach (var p in _participants)
        {
            foreach (var stock in p.Stocks ?? Enumerable.Empty<StockViewModel>())
            {
                var key = TickerToCacheKey(stock.Ticker, t);
                if (_historicalDataCache.TryGetValue(key, out var entry))
                    stock.HistoricalValues = entry.Data ?? new List<ValuePoint>();
                else
                    stock.HistoricalValues = new List<ValuePoint>();
            }
        }
    }

    private async Task<List<ValuePoint>> GetValueForDateTimes(ParticipantViewModel p, List<DateTime> dates, TimeRange timeRange, string targetCurrency = "DKK")
    {
        var result = new List<ValuePoint>();

        if (p == null)
            return result;

        foreach (var date in dates)
        {
            decimal portfolioValue = 0;

            var holdingsOnDate = p.Transactions?
                .Where(t => t.Date.Date <= date.Date)
                .GroupBy(t => t.Ticker)
                .Select(g => new
                {
                    Ticker = g.Key,
                    Amount = g.Sum(t => t.IsBuy ? (decimal)t.Amount : -(decimal)t.Amount)
                }) ?? Enumerable.Empty<dynamic>();

            foreach (var holding in holdingsOnDate)
            {
                if (holding.Amount <= 0) continue;

                // Use the correct cache key for the current time range
                string key = TickerToCacheKey(holding.Ticker, timeRange);
                _historicalDataCache.TryGetValue(key, out CacheEntry entry);

                var tickerHistory = entry?.Data;

                if (tickerHistory != null && tickerHistory.Any())
                {
                    var pricePoint = tickerHistory.Where(h => h.Date.Date <= date.Date)
                                                  .OrderByDescending(h => h.Date)
                                                  .FirstOrDefault();

                    // Fall back to the earliest available price when the date is before historical data starts
                    pricePoint ??= tickerHistory.OrderBy(h => h.Date).First();

                    // Use current exchange rate from stock model if available; historical FX not implemented
                    var stockVM = p.Stocks?.FirstOrDefault(s => s.Ticker == holding.Ticker);
                    decimal rate = stockVM?.CurrentExchangeRate ?? 1;

                    portfolioValue += (pricePoint.Value * rate) * holding.Amount;
                }
            }

            decimal remainingCash = CalculateRemainingCash(date, p.Transactions ?? new List<TransactionViewModel>());
            portfolioValue += remainingCash;

            result.Add(new ValuePoint { Date = date, Value = portfolioValue });
        }

        return result;
    }

    private string TickerToCacheKey(string ticker, TimeRange t) => $"{ticker}_{t.Value}";

    // Utility: clear cache or adjust TTL
    public void ClearHistoryCache() => _historicalDataCache.Clear();

    public void SetCacheTtl(TimeSpan ttl) => _cacheTtl = ttl;

    // Public helper to update everything for a new time range: fetch history, set current
    // stock values (including exchange rates) and recalculate player time series.
    public async Task UpdateForTimeRange(TimeRange t)
    {
        if (t == null) t = TimeRange.FiveDays;
        _currentTimeRange = t;
        await SetStockValues(t);
    }
}
