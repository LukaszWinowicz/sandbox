using MassUpdate.Core.Interfaces;

namespace MassUpdate.Infrastructure;

// Klasa symuluje dostęp do bazy danych.
// W prawdziwej aplikacji łączyłaby się z SQL, Entity Framework, BigQuery, itp.
public class OrderDataService : IOrderDataService
{
    private readonly HashSet<string> _dbOrders =  new() { "PO2025-001", "PROD-00001" };
    private readonly HashSet<string> _dbCombinations = new() { "PO2025-001-1-1", "PROD-00001-10-5" };

    public bool OrderExists(string order)
    {
        return !string.IsNullOrEmpty(order) && _dbOrders.Contains(order);
    }

    public bool LineCombinationExists(string order, int line, int sequence)
    {
        if (string.IsNullOrEmpty(order)) return false;
        string combinationKey = $"{order}-{line}-{sequence}";
        return _dbCombinations.Contains(combinationKey);
    }
}
    