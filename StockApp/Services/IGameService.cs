using StockApp.Models;
using StockLib;

namespace StockApp.Services;

public interface IGameService
{
    Task<bool> CreateGame(GameViewModel model);
    Task<List<GameViewModel>> GetGames();
    Task AddPlayerToGame(string gameId, string email);
    Task<List<GameViewModel>> GetPlayerGames();
    Task<List<ParticipantViewModel>> GetPlayers(string gameId);
    Task<IEnumerable<YahooQuoteResult>> SearchStocks(string search, CancellationToken ct);
    Task<(bool Success, string? ErrorMessage)> AddTransaction(TransactionViewModel transaction, string gameId);
    Task<Quote> GetQuote(string ticker);
    Task<List<ValuePoint>> GetHistory(string ticker, TimeRange t);

    Task UpdatePlayerName(string participantId, string newName);

}