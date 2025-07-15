using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;

namespace KERP.Domain.Abstractions;

/// <summary>
/// Interfejs wzorca Unit of Work.
/// Odpowiada za zapis zmian w kontekście danych oraz zapewnia transakcyjne wykonanie.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Zapisuje wszystkie zmiany w kontekście danych jako jedną transakcję.
    /// </summary>
    /// <param name="cancellationToken">Token anulujący operację.</param>
    /// <returns>Task reprezentujący zakończenie operacji.</returns>
    Task SaveChangesAsync(CancellationToken cancellationToken);

}