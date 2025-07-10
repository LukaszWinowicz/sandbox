using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using KERP.Domain.Entities.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace KERP.Infrastructure.Persistence;

public class KerpDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<PurchaseOrderReceiptDateUpdateEntity> PurchaseOrderReceiptDateUpdates { get; set; }
    public DbSet<FactoryEntity> Factories { get; set; }
    public DbSet<PurchaseOrderEntity> PurchaseOrders { get; set; }

    public KerpDbContext(DbContextOptions<KerpDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // WAŻNE: Wywołanie base.OnModelCreating() jest kluczowe dla Identity.
        // Konfiguruje ono schemat potrzebny do obsługi tożsamości.
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
