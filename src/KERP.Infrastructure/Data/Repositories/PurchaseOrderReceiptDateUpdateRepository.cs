using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;

namespace KERP.Infrastructure.Data.Repositories;

/// <summary>
/// Implementacja repozytorium dla agregatu aktualizacji dat zamówień.
/// </summary>
public sealed class PurchaseOrderReceiptDateUpdateRepository : IPurchaseOrderReceiptDateUpdateRepository
{
    private readonly AppDbContext _dbContext;

    public PurchaseOrderReceiptDateUpdateRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <inheritdoc />
    public Task AddAsync(PurchaseOrderReceiptDateUpdateRequestEntity entity, CancellationToken cancellationToken)
    {
        return _dbContext.PurchaseOrderReceiptDateUpdates.AddAsync(entity, cancellationToken).AsTask();
    }
}