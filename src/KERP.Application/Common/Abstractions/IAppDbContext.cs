using KERP.Domain.Aggregates.PurchaseOrder;
using Microsoft.EntityFrameworkCore;

namespace KERP.Application.Common.Abstractions;

public interface IAppDbContext
{
    // Udostępniamy tylko te DbSet'y, których potrzebuje warstwa aplikacji
    DbSet<PurchaseOrderReceiptDateChangeRequest> PurchaseOrderReceiptDateChangeRequests { get; }

    // Udostępniamy też metodę do zapisu, która będzie używana przez nasz IUnitOfWork
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
