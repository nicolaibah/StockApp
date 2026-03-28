using System.Text.Json;

namespace StockTrackingApi.Services;


public class ExchangeRateService : IExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency)
    {
        string url = $"https://api.frankfurter.dev/v1/latest?base={fromCurrency.ToUpper()}&symbols={targetCurrency.ToUpper()}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        string jsonResponse = await response.Content.ReadAsStringAsync();

        // Use JsonDocument for a quick, low-allocation way to grab a nested value
        using var doc = JsonDocument.Parse(jsonResponse);
        var rates = doc.RootElement.GetProperty("rates");

        // Access the property by the string name provided in targetCurrency
        if (rates.TryGetProperty(targetCurrency.ToUpper(), out JsonElement rateValue))
        {
            return rateValue.GetDecimal();
        }

        throw new KeyNotFoundException($"Currency {targetCurrency} not found in response.");
    }
}
