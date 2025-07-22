using KERP.Domain.Common;

namespace KERP.Domain.Events.PurchaseOrder;

public record PurchaseOrderReceiptDateChangeRequestedEvent(int RequestId, string PurchaseOrder) : IDomainEvent
{
    public DateTime DateOccurred { get; } = DateTime.UtcNow;
}