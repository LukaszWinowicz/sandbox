using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Features.MassUpdate.PurchaseOrder.Queries.DTOs;
using KERP.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Data.Repositories.MassUpdate.PurchaseOrder;

public sealed class PurchaseOrderReceiptDateUpdateReadRepository : IPurchaseOrderReceiptDateUpdateReadRepository
{
    private readonly AppDbContext _dbContext;

    public PurchaseOrderReceiptDateUpdateReadRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PurchaseOrderReceiptDateUpdateRequestDto>> GetFilteredAsync(
        string? purchaseOrder,
        ReceiptDateUpdateType? dateType,
        int? factoryId,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.PurchaseOrderReceiptDateUpdates.AsQueryable();

        if (!string.IsNullOrEmpty(purchaseOrder))
            query = query.Where(x => x.PurchaseOrder == purchaseOrder);

        if (dateType is not null)
            query = query.Where(x => x.DateType == dateType);

        if (factoryId is not null)
            query = query.Where(x => x.FactoryId == factoryId);

        return await query
            .OrderByDescending(x => x.CreatedAtUtc)
            .Select(x => new PurchaseOrderReceiptDateUpdateRequestDto
            {
                PurchaseOrder = x.PurchaseOrder,
                LineNumber = x.LineNumber,
                Sequence = x.Sequence,
                NewReceiptDate = x.NewReceiptDate,
                DateType = x.DateType,
                UserId = x.UserId,
                CreatedAtUtc = x.CreatedAtUtc
            })
            .ToListAsync(cancellationToken);
    }
}
