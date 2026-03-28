namespace StockApp.Services;

public interface IExchangeRateService
{
    Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency);
}
