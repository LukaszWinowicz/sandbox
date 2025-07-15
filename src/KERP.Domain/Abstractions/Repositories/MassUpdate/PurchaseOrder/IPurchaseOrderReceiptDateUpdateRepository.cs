using KERP.Domain.Entities.MassUpdate.PurchaseOrder;

namespace KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;

/// <summary>
/// Repozytorium dla agregatu <see cref="PurchaseOrderReceiptDateUpdateRequestEntity"/>.
/// Odpowiada za zapis żądań aktualizacji daty przyjęcia.
/// </summary>
public interface IPurchaseOrderReceiptDateUpdateRepository
{
    /// <summary>
    /// Dodaje nowe żądanie do kolejki aktualizacji.
    /// </summary>
    Task AddAsync(PurchaseOrderReceiptDateUpdateRequestEntity entity, CancellationToken cancellationToken);
}