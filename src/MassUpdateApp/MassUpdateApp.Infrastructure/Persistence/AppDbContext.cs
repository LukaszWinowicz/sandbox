using MassUpdateApp.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace MassUpdateApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
 
    // Query Models(Read-only Data Sources)
    /// <summary>
    /// Represents the validation data source for Purchase Orders, 
    /// synchronized from an external system like BigQuery.
    /// Used for read-only validation queries.
    /// </summary>
    public DbSet<ValidationPurchaseOrderEntity> ValidationPurchaseOrders { get; set; }

    // Command Models (Application Write Tables)
    /// <summary>
    /// Stores user-submitted requests for updating Purchase Orders.
    /// This is the target for our write operations (Commands).
    /// </summary>
    public DbSet<PurchaseOrderUpdateRequestEntity> PurchaseOrderUpdateRequests { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Konfigurujemy klucz złożony dla naszej tabeli walidacyjnej
        modelBuilder.Entity<ValidationPurchaseOrderEntity>()
            .HasKey(e => new { e.PurchaseOrder, e.LineNumber, e.Sequence });
    }
}
