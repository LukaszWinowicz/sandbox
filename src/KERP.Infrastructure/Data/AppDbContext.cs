using KERP.Domain.Entities.Common;
using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KERP.Infrastructure.Data;

/// <summary>
/// Główny kontekst danych korzystający z IdentityDbContext dla użytkowników.
/// </summary>
public class AppDbContext : IdentityDbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    /// <summary>
    /// Tabela do zapisu żądań aktualizacji dat odbioru.
    /// </summary>
    public DbSet<PurchaseOrderReceiptDateUpdateRequestEntity> PurchaseOrderReceiptDateUpdates { get; set; }

    /// <summary>
    /// Tabela fabryk — konfiguracja systemowa.
    /// </summary>
    public DbSet<FactoryEntity> Factories { get; set; }  

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Mapowanie do schematu 'upd' — jeśli baza wspiera schematy
        builder.Entity<PurchaseOrderReceiptDateUpdateRequestEntity>().ToTable("PurchaseOrderReceiptDate", schema: "upd");
        builder.Entity<PurchaseOrderReceiptDateUpdateRequestEntity>().HasKey(e => e.Id);

        builder.Entity<FactoryEntity>().ToTable("Factories");
        // Możesz dodać konfiguracje typu HasKey, MaxLength itd.
    }
}
