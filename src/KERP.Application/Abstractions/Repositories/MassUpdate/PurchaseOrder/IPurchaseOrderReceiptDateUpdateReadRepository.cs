using KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;
using KERP.Domain.Enums;

namespace KERP.Application.Abstractions.Repositories.MassUpdate.PurchaseOrder;

public interface IPurchaseOrderReceiptDateUpdateReadRepository
{
    Task<List<PurchaseOrderReceiptDateUpdateRequestDto>> GetFilteredAsync(
        string? purchaseOrder,
        ReceiptDateUpdateType? dateType,
        int? factoryId,
        CancellationToken cancellationToken);
}