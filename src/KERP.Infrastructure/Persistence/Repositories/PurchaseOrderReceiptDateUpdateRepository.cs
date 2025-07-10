using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

namespace KERP.Infrastructure.Persistence.Repositories;

public class PurchaseOrderReceiptDateUpdateRepository : IPurchaseOrderReceiptDateUpdateRepository
{
    private readonly KerpDbContext _context;

    public PurchaseOrderReceiptDateUpdateRepository(KerpDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(PurchaseOrderReceiptDateUpdateEntity entity, CancellationToken cancellationToken = default)
    {
        // Dodajemy encję do kontekstu EF Core.
        // Zapis do bazy danych nastąpi dopiero po wywołaniu SaveChangesAsync() w UnitOfWork.
        await _context.PurchaseOrderReceiptDateUpdates.AddAsync(entity, cancellationToken);
    }
}
