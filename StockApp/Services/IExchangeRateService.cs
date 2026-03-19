namespace StockTrackingApi.Services;

public interface IExchangeRateService
{
    public Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency);


}
