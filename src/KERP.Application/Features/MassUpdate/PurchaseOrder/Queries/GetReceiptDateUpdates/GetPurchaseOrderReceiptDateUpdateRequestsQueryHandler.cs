using KERP.Application.Abstractions.CQRS;
using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;

namespace KERP.Application.Features.MassUpdate.PurchaseOrder.Query.GetReceiptDateUpdates;

public sealed class GetPurchaseOrderReceiptDateUpdateRequestsQueryHandler
    : IQueryHandler<GetPurchaseOrderReceiptDateUpdateRequestsQuery, List<PurchaseOrderReceiptDateUpdateRequestDto>>
{
    private readonly IPurchaseOrderReceiptDateUpdateReadRepository _repository;

    public GetPurchaseOrderReceiptDateUpdateRequestsQueryHandler(IPurchaseOrderReceiptDateUpdateReadRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<PurchaseOrderReceiptDateUpdateRequestDto>> HandleAsync(
        GetPurchaseOrderReceiptDateUpdateRequestsQuery query,
        CancellationToken cancellationToken)
    {
        return await _repository.GetFilteredAsync(
            query.PurchaseOrderFilter,
            query.DateTypeFilter,
            query.FactoryIdFilter,
            cancellationToken);
    }
}