using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Common.Context;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Data.Repositories.Common;

public class ExternalPurchaseOrderRepository : IExternalPurchaseOrderRepository
{
    private readonly AppDbContext _context;
    private readonly ICurrentUserContext _userContext;
    public ExternalPurchaseOrderRepository(AppDbContext context, ICurrentUserContext userContext)
    {
        _context = context;
        _userContext = userContext;
    }

    public async Task<bool> ExistsAsync(string purchaseOrder, CancellationToken cancellationToken = default)
    {
        // Pobieranie fabryki z kontekstu
        var factory = await _context.Factories
            .FirstOrDefaultAsync(f => f.Id == _userContext.FactoryId, cancellationToken);

        if (factory == null)
            throw new InvalidOperationException($"Nieznana fabryka: {_userContext.FactoryId}");

        // Dynamiczne zapytanie SQL
        var tableName = $"bgq.PurchaseOrder_{factory.Id}";
        var sql = $"SELECT COUNT(1) FROM {tableName} WHERE PurchaseOrder = @p0";

        var count = await _context.Database
            .SqlQueryRaw<int>(sql, purchaseOrder)
            .FirstOrDefaultAsync(cancellationToken);

        return count > 0;
    }

    public async Task<bool> ExistsOrderLineAsync(
            string purchaseOrder,
            int lineNumber,
            int sequence,
            CancellationToken cancellationToken)
    {
        var factory = await _context.Factories
            .FirstOrDefaultAsync(f => f.Id == _userContext.FactoryId, cancellationToken);

        var tableName = $"bgq.PurchaseOrder_{factory.Id}";
        var sql = @$"
            SELECT COUNT(1) FROM {tableName} 
            WHERE PurchaseOrder = @p0 
              AND LineNumber = @p1 
              AND Sequence = @p2";

        var count = await _context.Database
            .SqlQueryRaw<int>(sql, purchaseOrder, lineNumber, sequence)
            .FirstOrDefaultAsync(cancellationToken);

        return count > 0;
    }
}