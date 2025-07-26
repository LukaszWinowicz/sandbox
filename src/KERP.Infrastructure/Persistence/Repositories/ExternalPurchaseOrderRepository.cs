using KERP.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Persistence.Repositories;

/// <summary>
/// Implementacja repozytorium dla zamówień zakupu.
/// </summary>
public class ExternalPurchaseOrderRepository : IExternalPurchaseOrderRepository
{
    private readonly AppDbContext _context;

    public ExternalPurchaseOrderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetExistingPurchaseOrdersAsync(IEnumerable<string> purchaseOrders, CancellationToken cancellationToken)
    {
        // Wykonuje jedno, zoptymalizowane zapytanie do bazy danych, aby unikać problemu N-1.
        var existingOrders = await _context.PurchaseOrders
            .Where(po => purchaseOrders.Contains(po.PurchaseOrder))
            .Select(po => po.PurchaseOrder)
            .ToListAsync(cancellationToken);

        return new HashSet<string>(existingOrders, StringComparer.OrdinalIgnoreCase);
    }
}
