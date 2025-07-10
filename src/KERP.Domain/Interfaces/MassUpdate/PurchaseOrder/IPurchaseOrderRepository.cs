namespace KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

public interface IPurchaseOrderRepository
{
    Task<bool> OrderExistsAsync(string purchaseOrderNumber, int factoryId, CancellationToken cancellationToken = default);

    Task<bool> CombinationExistsAsync(string purchaseOrder, int lineNumber, int sequence, int factoryId, CancellationToken cancellationToken = default);
}
