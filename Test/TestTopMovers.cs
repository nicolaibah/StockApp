using StockApp.Models;
using StockLib;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;

namespace Test;

public class TestTopMovers
{
    [Fact]
    public void TopMoversTest()
    {
        string ticker = "NOVO-B.CO";
        decimal currentValue = 229m;
        List<ValuePoint> values = JsonSerializer.Deserialize<List<ValuePoint>>(JsonHist());
        TimeRange t = TimeRange.OneMonth;
        GetGain(t, currentValue, values);
    }

    private void GetGain(TimeRange t, decimal currentValue, List<ValuePoint> vps)
    {
        ValuePoint historicalPoint = GetValuePoint(t, vps);

        if (historicalPoint == null)
        {
            Console.WriteLine($"No data found for the range: {t.Value}");
            return;
        }

        // Standard Growth Formula: ((New - Old) / Old) * 100
        decimal gain = ((currentValue - historicalPoint.Value) / historicalPoint.Value) * 100;

        Console.WriteLine($"{t.Value} Change: {gain:0.00}% (From {historicalPoint.Value} on {historicalPoint.Date:d})");
    }

    private ValuePoint GetValuePoint(TimeRange t, List<ValuePoint> vps)
    {
        if (vps == null || vps.Count == 0) return null;

        // Ensure chronological order (Oldest -> Newest)
        var sorted = vps.OrderBy(x => x.Date).ToList();
        int count = sorted.Count;

        return t.Value switch
        {
            // ^1 is current, ^2 is 1d ago (yesterday)
            "1d" => count >= 2 ? sorted[^2] : sorted[0],

            // As requested: 5d is the 4th last element
            "5d" => count >= 4 ? sorted[^4] : sorted[0],

            // Calendar-based logic for longer windows
            "1m" => GetByCalendar(sorted, DateTime.Now.AddMonths(-1)),
            "3m" => GetByCalendar(sorted, DateTime.Now.AddMonths(-3)),
            "1y" => GetByCalendar(sorted, DateTime.Now.AddYears(-1)),

            _ => count >= 2 ? sorted[^2] : sorted[0]
        };
    }
    private ValuePoint GetByCalendar(List<ValuePoint> sorted, DateTime targetDate)
    {
        // Find the latest trading day that is on or before our target calendar date
        // (Excludes today to ensure we aren't comparing today to today)
        return sorted
            .Where(x => x.Date.Date < DateTime.Now.Date)
            .LastOrDefault(x => x.Date.Date <= targetDate.Date)
            ?? sorted[0];
    }

    private string JsonHist()
    {
        return """"
            [{"Date":"2026-02-24T08:00:00","Value":243.64999389648438},{"Date":"2026-02-25T08:00:00","Value":238.39999389648438},{"Date":"2026-02-26T08:00:00","Value":238.60000610351562},{"Date":"2026-02-27T08:00:00","Value":237.89999389648438},{"Date":"2026-03-02T08:00:00","Value":237.35000610351562},{"Date":"2026-03-03T08:00:00","Value":231.5},{"Date":"2026-03-04T08:00:00","Value":245.4499969482422},{"Date":"2026-03-05T08:00:00","Value":248.64999389648438},{"Date":"2026-03-06T08:00:00","Value":247.89999389648438},{"Date":"2026-03-09T08:00:00","Value":254.5},{"Date":"2026-03-10T08:00:00","Value":246.9499969482422},{"Date":"2026-03-11T08:00:00","Value":249.8000030517578},{"Date":"2026-03-12T08:00:00","Value":247},{"Date":"2026-03-13T08:00:00","Value":247.0500030517578},{"Date":"2026-03-16T08:00:00","Value":248.3000030517578},{"Date":"2026-03-17T08:00:00","Value":249.14999389648438},{"Date":"2026-03-18T08:00:00","Value":241.5},{"Date":"2026-03-19T08:00:00","Value":237.89999389648438},{"Date":"2026-03-20T08:00:00","Value":237.4499969482422},{"Date":"2026-03-24T08:00:00","Value":238},{"Date":"2026-03-25T08:00:00","Value":235.8000030517578},{"Date":"2026-03-26T08:40:27","Value":229.60000610351562}]
            """";
    }
}
