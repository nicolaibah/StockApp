using StockApp.Models;
using StockTrackingApi.Models.StockModels;
using StockTrackingApi.Services;
using static StockApp.Pages.Game;

namespace StockApp.Services;

public class PresentationService
{
    private readonly IExchangeRateService _exchangeService;
    private readonly IGameService _gameService;
    public PresentationService(IExchangeRateService exchangeService, IGameService gameService)
    {
        _exchangeService = exchangeService;
        _gameService = gameService;
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
    public async Task<List<ValuePoint>> GetPortfolioHistory(IEnumerable<StockViewModel> stocks, string targetCurrency = "DKK")
    {
        var allPoints = new List<ValuePoint>();

        foreach (var stock in stocks)
        {
            // 1. Get Currency info
            Quote q = await _gameService.GetQuote(stock.Ticker);

            // 2. Get Historical Prices (Native Currency)
            var history = await _gameService.GetHistory(stock.Ticker);

            // 3. Get Exchange Rate
            decimal rate = 1;
            if (q.Currency != targetCurrency)
            {
                rate = await _exchangeService.ExchangeRate(q.Currency, targetCurrency);
            }

            // 4. Convert and add to flat list
            foreach (var vp in history)
            {
                allPoints.Add(new ValuePoint
                {
                    Date = vp.Date,
                    Value = (vp.Value * rate) * stock.Amount
                });
            }
        }

        // 5. Group by Date and Sum to create the single portfolio line
        return allPoints
            .GroupBy(p => p.Date.Date)
            .Select(g => new ValuePoint
            {
                Date = g.Key,
                Value = g.Sum(p => p.Value)
            })
            .OrderBy(p => p.Date)
            .ToList();
    }
}
