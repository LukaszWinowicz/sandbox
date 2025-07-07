using KERP.Core.Features.MassUpdate.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Persistence;

/// <summary>
/// The main database context for the application, based on Entity Framework Core.
/// It includes DbSets for application-specific entities and Identity tables.
/// </summary>
public class AppDbContext : IdentityDbContext<IdentityUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    #region Command Models (Application Write Tables)

    /// <summary>
    /// Stores user-submitted requests for updating Purchase Order Receipt Dates.
    /// </summary>
    public DbSet<PurchaseOrderReceiptDateUpdateEntity> PurchaseOrderReceiptDateUpdates { get; set; }

    #endregion

    #region Query Models (Read-only Data Sources)

    /// <summary>
    /// Represents the validation data source for Purchase Orders,
    /// intended to be synchronized from an external system like BigQuery.
    /// </summary>
    public DbSet<ValidationPurchaseOrderEntity> ValidationPurchaseOrders { get; set; }

    #endregion

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ValidationPurchaseOrderEntity>()
            .HasKey(e => new { e.PurchaseOrder, e.LineNumber, e.Sequence });
    }
}