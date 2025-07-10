using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KERP.Infrastructure.Persistence;

public class KerpDbContext : DbContext
{
    public DbSet<PurchaseOrderReceiptDateUpdateEntity> PurchaseOrderReceiptDateUpdates { get; set; }
    public DbSet<FactoryEntity> Factories { get; set; }
    public DbSet<PurchaseOrderEntity> PurchaseOrders { get; set; }

    public KerpDbContext(DbContextOptions<KerpDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Ta linia automatycznie skanuje i stosuje wszystkie konfiguracje
        // zdefiniowane w naszym projekcie (jak PurchaseOrderReceiptDateUpdateConfiguration)
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
