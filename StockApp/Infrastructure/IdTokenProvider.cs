using Microsoft.JSInterop;
using System.Text.Json;

namespace StockApp.Infrastructure;

public class IdTokenProvider
{
    private readonly IJSRuntime _js;
    private readonly string _clientId;

    public IdTokenProvider(IJSRuntime js, IConfiguration config)
    {
        _js = js;
        // This matches the key Google's library uses in SessionStorage
        _clientId = config["Google:ClientId"];
    }

    public async Task<string?> GetIdTokenAsync()
    {
        // The key format for the OIDC library is usually:
        // oidc.user:https://accounts.google.com:[ClientId]
        var key = $"oidc.user:https://accounts.google.com:{_clientId}";
        var data = await _js.InvokeAsync<string>("sessionStorage.getItem", key);

        if (string.IsNullOrEmpty(data)) return null;

        using var doc = JsonDocument.Parse(data);
        return doc.RootElement.TryGetProperty("id_token", out var token)
               ? token.GetString()
               : null;
    }
}
