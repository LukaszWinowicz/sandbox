using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

public class PurchaseOrderReceiptDateUpdateConfiguration : IEntityTypeConfiguration<PurchaseOrderReceiptDateUpdateEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceiptDateUpdateEntity> builder)
    {
        // Definiujemy nazwę tabeli i schemat
        builder.ToTable("PurchaseOrderReceiptDate", "upd");

        // Definiujemy klucz główny
        builder.HasKey(e => e.Id);

        // Konfigurujemy właściwości
        builder.Property(e => e.PurchaseOrder)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(e => e.DateType)
            .IsRequired()
            .HasConversion<string>() // Zapisujemy enum jako string w bazie
            .HasMaxLength(50);
    }
}