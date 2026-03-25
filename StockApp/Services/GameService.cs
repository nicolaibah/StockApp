using StockApp.Models;
using System.Net.Http.Json;
using System.Transactions;

namespace StockApp.Services;

public class GameService : IGameService
{
    private HttpClient _client;
    public GameService(IHttpClientFactory _factory)
    {
        _client = _factory.CreateClient("Api");
    }

    public async Task<bool> CreateGame(GameViewModel model)
    {
        try
        {
            var response = await _client.PostAsJsonAsync("api/stock/games", model);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fejl ved oprettelse af spil: {ex.Message}");
            return false;
        }
    }
    public async Task<List<GameViewModel>> GetGames()
    {
        List<GameViewModel> games = await _client.GetFromJsonAsync<List<GameViewModel>>("api/stock/games");
        return games;
    }
    public async Task AddPlayerToGame(string gameId, string email)
    {
        await _client.PostAsJsonAsync($"api/stock/player/games?gameId={gameId}", email);
    }
    public async Task<List<GameViewModel>> GetPlayerGames()
    {
        List<GameViewModel> games = await _client.GetFromJsonAsync<List<GameViewModel>>("api/stock/player/games");
        return games;
    }
    public async Task<List<ParticipantViewModel>> GetPlayers(string gameId)
    {
        List<ParticipantViewModel> players = await _client.GetFromJsonAsync<List<ParticipantViewModel>>($"api/stock/games/players?gameId={gameId}");
        return players;
    }
    public async Task<IEnumerable<YahooQuoteResult>> SearchStocks(string search, CancellationToken _)
    {
        List<YahooQuoteResult> symbols = await _client.GetFromJsonAsync<List<YahooQuoteResult>>($"api/stock/search?search={search}");
        return symbols;
    }
    public async Task<(bool Success, string? ErrorMessage)> AddTransaction(TransactionViewModel transaction, string gameId)
    {
        var response = await _client.PostAsJsonAsync($"api/stock/transaction?gameId={gameId}", transaction);

        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var errorMsg = await response.Content.ReadAsStringAsync();
        return (false, errorMsg);
    }
    public async Task<Quote> GetQuote(string ticker)
    {
        return await _client.GetFromJsonAsync<Quote>($"api/stock/quote?ticker={ticker}");
    }
    public async Task<List<ValuePoint>> GetHistory(string ticker, DateTime fromDate)
    {

        var res = await _client.GetFromJsonAsync<List<ValuePoint>>($"api/stock/quote/history?ticker={ticker}&fromDate={fromDate.Ticks}");
        return res ?? new List<ValuePoint>();

    }
}
