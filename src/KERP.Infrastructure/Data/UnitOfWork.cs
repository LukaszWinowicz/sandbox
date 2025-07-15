using KERP.Domain.Abstractions;
using KERP.Domain.Abstractions.Repositories.MassUpdate.PurchaseOrder;

namespace KERP.Infrastructure.Data;

/// <summary>
/// Implementacja Unit of Work na bazie EF Core.
/// Operuje na <see cref="AppDbContext"/>.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public IPurchaseOrderReceiptDateUpdateRepository PurchaseOrderUpdates { get; }

    public UnitOfWork(AppDbContext dbContext, IPurchaseOrderReceiptDateUpdateRepository repo)
    {
        _dbContext = dbContext;
        PurchaseOrderUpdates = repo;
    }

    /// <inheritdoc />
    public Task SaveChangesAsync(CancellationToken cancellationToken)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }
}