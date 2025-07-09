using KERP.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implements the repository for fetching read-only data
/// specifically for Purchase Order validation.
/// </summary>
public class PurchaseOrderValidationRepository : IPurchaseOrderValidationRepository
{
    private readonly AppDbContext _context;

    public PurchaseOrderValidationRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public async Task<bool> OrderExistsAsync(string orderNumber)
    {
        // Uses EF Core's LINQ extension method to efficiently check for existence.
        // This translates to an "EXISTS" query in SQL.
        return await _context.ValidationPurchaseOrders
            .AnyAsync(p => p.PurchaseOrder == orderNumber);
    }

    /// <inheritdoc />
    public async Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence)
    {
        return await _context.ValidationPurchaseOrders
            .AnyAsync(p => p.PurchaseOrder == orderNumber
                        && p.LineNumber == lineNumber
                        && p.Sequence == sequence);
    }
}
