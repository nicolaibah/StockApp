using System.Text.Json;

namespace StockApp.Services;

public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency)
    {
        string from = fromCurrency.ToUpper();
        string target = targetCurrency.ToUpper();
        string url = $"https://api.frankfurter.dev/v1/latest?base={from}&symbols={target}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(jsonResponse);
        var rates = doc.RootElement.GetProperty("rates");

        if (rates.TryGetProperty(target, out JsonElement rateValue))
        {
            return rateValue.GetDecimal();
        }

        throw new KeyNotFoundException($"Currency {targetCurrency} not found in response.");
    }
}
