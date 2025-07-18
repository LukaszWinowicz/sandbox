using KERP.Application.Abstractions.Queries.Repositories;
using KERP.Application.Common.Context;
using KERP.Domain.Entities.Common;
using Microsoft.Data.SqlClient;
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
        if (string.IsNullOrWhiteSpace(purchaseOrder))
        {
            return false;
        }

        var trimmedPurchaseOrder = purchaseOrder.Trim();

        try
        {
            var factory = await _context.Factories
                   .AsNoTracking()
                   .FirstOrDefaultAsync(f => f.Id == _userContext.FactoryId, cancellationToken);

            if (factory == null)
            {
                throw new InvalidOperationException($"Nie można odnaleźć konfiguracji fabryki o ID: {_userContext.FactoryId}");
            }

            var tableName = $"bgq.PurchaseOrder_{factory.Id}";
            var sql = $"SELECT COUNT(1) AS ResultCount FROM {tableName} WHERE TRIM(PurchaseOrderNumber) = @poNumber";
            var sqlParameter = new SqlParameter("@poNumber", trimmedPurchaseOrder);

            // POPRAWKA: Używamy SqlQueryRaw, która akceptuje surowy string i parametry.
            var result = await _context.Database
                .SqlQueryRaw<PurchaseOrderValidationData>(sql, sqlParameter)
                .FirstOrDefaultAsync(cancellationToken);

            return result?.ResultCount > 0;
        }
        catch (SqlException ex)
        {
            Console.WriteLine($"Błąd SQL podczas sprawdzania istnienia zamówienia '{trimmedPurchaseOrder}': {ex.Message}");
            return false;
        }
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

public class PurchaseOrderValidationData
{
    /// <summary>
    /// Przechowuje wynik zapytania COUNT(*).
    /// Nazwa właściwości musi pasować do aliasu kolumny w zapytaniu SQL (np. SELECT COUNT(1) AS ResultCount...).
    /// </summary>
    public int ResultCount { get; set; }
}