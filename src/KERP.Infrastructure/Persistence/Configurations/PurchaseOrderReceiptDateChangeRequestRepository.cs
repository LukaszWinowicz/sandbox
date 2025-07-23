using KERP.Domain.Aggregates.PurchaseOrder;

namespace KERP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Konkretna implementacja repozytorium dla agregatu PurchaseOrderReceiptDateChangeRequest.
/// </summary>
public class PurchaseOrderReceiptDateChangeRequestRepository
    : IPurchaseOrderReceiptDateChangeRequestRepository
{
    private readonly AppDbContext _context;

    public PurchaseOrderReceiptDateChangeRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public void Add(PurchaseOrderReceiptDateChangeRequest request)
    {
        // Dodaje obiekt do Change Trackera w EF Core.
        // Zapis w bazie nastąpi dopiero po wywołaniu SaveChangesAsync().
        _context.Set<PurchaseOrderReceiptDateChangeRequest>().Add(request);
    }
}