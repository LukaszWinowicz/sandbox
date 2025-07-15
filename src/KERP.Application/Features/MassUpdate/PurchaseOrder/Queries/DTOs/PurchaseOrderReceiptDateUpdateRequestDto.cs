using KERP.Domain.Enums;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;

public sealed class PurchaseOrderReceiptDateUpdateRequestDto
{
    public string PurchaseOrder { get; init; }
    public int LineNumber { get; init; }
    public int Sequence { get; init; }
    public DateTime NewReceiptDate { get; init; }
    public ReceiptDateUpdateType DateType { get; init; }
    public int FactoryId { get; init; }
    public string UserId { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public bool IsProcessed { get; init; }
    public DateTime? ProcessedDate { get; init; }
}
