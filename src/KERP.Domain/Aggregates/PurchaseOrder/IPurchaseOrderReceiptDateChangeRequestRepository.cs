namespace KERP.Domain.Aggregates.PurchaseOrder;

/// <summary>
/// Definiuje kontrakt dla repozytorium zarządzającego agregatami
/// <see cref="PurchaseOrderReceiptDateChangeRequest"/>.
/// </summary>
public interface IPurchaseOrderReceiptDateChangeRequestRepository
{
    /// <summary>
    /// Dodaje nowe żądanie zmiany do repozytorium, aby zostało utrwalone.
    /// </summary>
    /// <param name="request">Obiekt żądania do dodania.</param>
    void Add(PurchaseOrderReceiptDateChangeRequest request);
}
