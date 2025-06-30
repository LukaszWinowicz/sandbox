using MassUpdate.Core.Interfaces.Repositories;

namespace MassUpdate.Infrastructure.Persistence.Repositories;

public class OrderRepository : IPurchaseOrderValidationRepository
{   
    public Task<bool> OrderExistsAsync(string orderNumber)
    {
        // TODO: W następnym kroku zaimplementujemy tutaj prawdziwe zapytanie do bazy danych.
        // Na razie zwracamy prostą logikę do testów.
        bool exists = orderNumber == "PO2025-001" || orderNumber == "PROD-123";
        return Task.FromResult(exists);
    }

    public Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence)
    {
        // TODO: W następnym kroku zaimplementujemy tutaj prawdziwe zapytanie do bazy danych.
        bool exists = orderNumber == "PO2025-001" && lineNumber == 10 && sequence == 1;
        return Task.FromResult(exists);
    }

}
