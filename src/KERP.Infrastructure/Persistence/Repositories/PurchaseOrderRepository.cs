using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Persistence.Repositories;

public class PurchaseOrderRepository : IPurchaseOrderRepository
{
    private readonly KerpDbContext _context;

    public PurchaseOrderRepository(KerpDbContext context)
    {
        _context = context;
    }

    public async Task<bool> OrderExistsAsync(string purchaseOrderNumber, int factoryId, CancellationToken cancellationToken = default)
    {
        // Nie musimy tu używać 'factoryId'!
        // Nasz Interceptor automatycznie podmieni tabelę w zapytaniu SQL.
        return await _context.PurchaseOrders
            .AnyAsync(po => po.PurchaseOrderNumber == purchaseOrderNumber, cancellationToken);
    }

    public async Task<bool> CombinationExistsAsync(string purchaseOrder, int lineNumber, int sequence, int factoryId, CancellationToken cancellationToken = default)
    {
        // Tutaj również nie używamy 'factoryId'. Interceptor robi całą magię.
        return await _context.PurchaseOrders
            .AnyAsync(po => po.PurchaseOrderNumber == purchaseOrder &&
                            po.LineNumber == lineNumber &&
                            po.Sequence == sequence,
                      cancellationToken);
    }
}