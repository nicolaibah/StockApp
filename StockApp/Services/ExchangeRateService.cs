using System.Text.Json;

namespace StockTrackingApi.Services;


public class ExchangeRateService : IExchangeRateService
{
    public async Task<decimal> ExchangeRate(string fromCurrency, string targetCurrency)
    {
        string url = $"https://api.frankfurter.dev/v1/1999-01-04?base={fromCurrency.ToUpper()}&symbols={targetCurrency.ToUpper()}";

        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync(url);
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
