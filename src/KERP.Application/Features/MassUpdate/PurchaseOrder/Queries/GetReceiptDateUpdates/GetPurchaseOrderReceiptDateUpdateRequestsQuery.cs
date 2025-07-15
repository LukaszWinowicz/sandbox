using KERP.Application.Abstractions.CQRS;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;
using KERP.Domain.Enums;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Query.GetReceiptDateUpdates;

public sealed class GetPurchaseOrderReceiptDateUpdateRequestsQuery : IQuery<List<PurchaseOrderReceiptDateUpdateRequestDto>>
{
    public string? PurchaseOrderFilter { get; init; }
    public ReceiptDateUpdateType? DateTypeFilter { get; init; }
    public int? FactoryIdFilter { get; init; }
}
