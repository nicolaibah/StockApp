using StockApp.Models;

namespace StockApp.Services;

public interface IPresentationService
{
    string TargetCurrency { get; set; }
    Task Init(List<ParticipantViewModel> participants, decimal gameCapital);
    Task Init(List<ParticipantViewModel> participants, decimal gameCapital, TimeRange t);
    Task SetTickerHistory(TimeRange t);
    Task UpdateForTimeRange(TimeRange t);
    void ClearHistoryCache();
    void SetCacheTtl(TimeSpan ttl);
}
