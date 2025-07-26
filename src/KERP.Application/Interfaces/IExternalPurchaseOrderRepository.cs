namespace KERP.Application.Interfaces;

/// <summary>
/// Definiuje operacje na danych dla zamówień zakupu.
/// </summary>
public interface IExternalPurchaseOrderRepository
{
    /// <summary>
    /// Sprawdza, które z podanych numerów zamówień zakupu istnieją w bazie danych.
    /// </summary>
    /// <param name="purchaseOrders">Kolekcja numerów zamówień do sprawdzenia.</param>
    /// <param name="cancellationToken">Token do anulowania operacji.</param>
    /// <returns>Zbiór (HashSet) numerów zamówień, które istnieją w systemie.</returns>
    Task<HashSet<string>> GetExistingPurchaseOrdersAsync(IEnumerable<string> purchaseOrders, CancellationToken cancellationToken);
}
