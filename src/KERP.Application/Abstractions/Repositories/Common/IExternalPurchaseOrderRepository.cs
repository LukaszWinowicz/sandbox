namespace KERP.Application.Abstractions.Repositories.Common;

/// <summary>
/// Abstrakcja repozytorium dla danych zamówień z systemów zewnętrznych (np. BigQuery).
/// Służy wyłącznie do celów walidacyjnych i odczytu.
/// </summary>
public interface IExternalPurchaseOrderRepository
{
    /// <summary>
    /// Sprawdza, czy zamówienie o danym numerze istnieje dla określonej fabryki.
    /// </summary>
    /// <param name="purchaseOrder">Numer zamówienia do sprawdzenia.</param>
    /// <param name="cancellationToken">Token do anulowania operacji.</param>
    /// <returns>True, jeśli zamówienie istnieje; w przeciwnym razie false.</returns>
    Task<bool> ExistsAsync(string purchaseOrder, CancellationToken cancellationToken = default);
}