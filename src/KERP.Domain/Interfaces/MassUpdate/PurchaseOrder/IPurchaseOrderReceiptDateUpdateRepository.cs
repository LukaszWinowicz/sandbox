using KERP.Domain.Entities.MassUpdate.PurchaseOrder;

namespace KERP.Domain.Interfaces.MassUpdate.PurchaseOrder;

public interface IPurchaseOrderReceiptDateUpdateRepository
{    
    Task AddAsync(PurchaseOrderReceiptDateUpdateEntity entity, CancellationToken cancellationToken = default);
}
