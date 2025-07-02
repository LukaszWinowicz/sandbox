using MassUpdateApp.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MassUpdateApp.Infrastructure.Persistence.Repositories;

public class ValidationDataRepository : IValidationDataRepository
{
    private readonly AppDbContext _context;

    public ValidationDataRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> PurchaseOrderExistsAsync(string orderNumber)
    {
        return await _context.ValidationPurchaseOrders
            .AnyAsync(po => po.PurchaseOrder == orderNumber);
    }

    public async Task<bool> CombinationExistsAsync(string orderNumber, int lineNumber, int sequence)
    {
        return await _context.ValidationPurchaseOrders
            .AnyAsync(po => po.PurchaseOrder == orderNumber &&
                             po.LineNumber == lineNumber &&
                             po.Sequence == sequence);
    }
}
