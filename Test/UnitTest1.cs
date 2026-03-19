using Moq;
using StockApp.Models;
using StockApp.Services;
using StockTrackingApi.Models.StockModels;
using StockTrackingApi.Services;
using static StockApp.Pages.Game;

namespace Test;

public class UnitTest1
{
    private readonly PresentationService _presentationService;
    private DateTime day1 = new DateTime(2026, 01, 01);
    private DateTime day2 = new DateTime(2026, 01, 02);
    private DateTime day3 = new DateTime(2026, 01, 03);
    private DateTime day4 = new DateTime(2026, 01, 04);
    private DateTime day5 = new DateTime(2026, 01, 05);

    public UnitTest1()
    {
        var _gameServiceMock = new Mock<IGameService>();

        // 2. Setup the mock behavior for GetHistory
        var mockDataMsft = new List<ValuePoint>
        {
            new() {  Date = day1, Value = 1  },
            new() {  Date = day2, Value = 2  },
            new() {  Date = day3, Value = 3  },
            new() {  Date = day4, Value = 4  },
            new() {  Date = day5,  Value = 5}

        };
        var mockDataNovob = new List<ValuePoint>
        {
            new() {  Date = day1, Value = 1  },
            new() {  Date = day2, Value = 2  },
            new() {  Date = day3, Value = 3  },
            new() {  Date = day4, Value = 4  },
            new() {  Date = day5,  Value = 5}

        };
        Quote msftQuote = new()
        {
            Currency = "USD"
        };
        Quote novobQuote = new()
        {
            Currency = "DKK"
        };
        _gameServiceMock
            .Setup(s => s.GetHistory("MSFT"))
            .ReturnsAsync(mockDataMsft);
        _gameServiceMock
            .Setup(s => s.GetHistory("NOVO-B.CO"))
            .ReturnsAsync(mockDataNovob);
        _gameServiceMock.Setup(s => s.GetQuote("MSFT"))
            .ReturnsAsync(msftQuote);
        _gameServiceMock.Setup(s => s.GetQuote("NOVO-B.CO"))
            .ReturnsAsync(novobQuote);

        // 3. Inject the MOCK into your PresentationService
        // (Assuming PresentationService takes IGameService in its constructor)

        IExchangeRateService exchangeRateService = new MockExchangeRateService();
        _presentationService = new(exchangeRateService, _gameServiceMock.Object);
    }
    [Fact]
    public async Task Test()
    {

        //MSFT only
        StockViewModel microsoft = new StockViewModel()
        {
            Amount = 4,
            Ticker = "MSFT",
        };
        List<StockViewModel> list = [microsoft];
        List<ValuePoint> history = await _presentationService.GetPortfolioHistory(list);
        decimal dollarRate = 6.5m;
        Assert.Equal(4 * 1 * dollarRate,history.First(x => x.Date == day1).Value);
        Assert.Equal(4 * 2 * dollarRate, history.First(x => x.Date == day2).Value);
        Assert.Equal(4 * 3 * dollarRate, history.First(x => x.Date == day3).Value);
        Assert.Equal(4 * 4 * dollarRate, history.First(x => x.Date == day4).Value);
        Assert.Equal(4 * 5 * dollarRate, history.First(x => x.Date == day5).Value);

        // NOVO-B.CO with DKK currency (base currency)
        StockViewModel novob = new StockViewModel()
        {
            Amount = 2,
            Ticker = "NOVO-B.CO",
        };
        List<StockViewModel> listWithNovob = [novob];
        List<ValuePoint> historyNovob = await _presentationService.GetPortfolioHistory(listWithNovob);
        Assert.Equal(2 * 1, historyNovob.First(x => x.Date == day1).Value);
        Assert.Equal(2 * 2, historyNovob.First(x => x.Date == day2).Value);
        Assert.Equal(2 * 3, historyNovob.First(x => x.Date == day3).Value);
        Assert.Equal(2 * 4, historyNovob.First(x => x.Date == day4).Value);
        Assert.Equal(2 * 5, historyNovob.First(x => x.Date == day5).Value);

        // Combined portfolio with both MSFT and NOVO-B.CO
        List<StockViewModel> combinedList = [microsoft, novob];
        List<ValuePoint> combinedHistory = await _presentationService.GetPortfolioHistory(combinedList);
        Assert.Equal((4 * 1 * dollarRate) + (2 * 1), combinedHistory.First(x => x.Date == day1).Value);
        Assert.Equal((4 * 2 * dollarRate) + (2 * 2), combinedHistory.First(x => x.Date == day2).Value);
        Assert.Equal((4 * 3 * dollarRate) + (2 * 3), combinedHistory.First(x => x.Date == day3).Value);
        Assert.Equal((4 * 4 * dollarRate) + (2 * 4), combinedHistory.First(x => x.Date == day4).Value);
        Assert.Equal((4 * 5 * dollarRate) + (2 * 5), combinedHistory.First(x => x.Date == day5).Value);


    }

}

public class MockExchangeRateService : IExchangeRateService
{
    public Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency)
    {
        // Handle the case where they are the same
        if (fromCurrency == targetCurrency) return Task.FromResult(1.0m);

        if (fromCurrency == "DKK" && targetCurrency == "USD")
        {
            // 1 / 6.5
            return Task.FromResult(1m / 6.5m);
        }
        else if (fromCurrency == "USD" && targetCurrency == "DKK")
        {
            // 1 * 6.5
            return Task.FromResult(6.5m);
        }

        // Default or "Error" case for the mock
        return Task.FromResult(0m);
    }
}