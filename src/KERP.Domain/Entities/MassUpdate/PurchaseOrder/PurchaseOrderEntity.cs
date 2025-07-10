using KERP.Domain.Interfaces.Shared;

namespace KERP.Domain.Entities.MassUpdate.PurchaseOrder;

// Oznaczamy encję naszym nowym interfejsem
public class PurchaseOrderEntity : ITenantedByFactory
{
    public required string PurchaseOrderNumber { get; set; }
    public int LineNumber { get; set; }
    public int Sequence { get; set; }
}
