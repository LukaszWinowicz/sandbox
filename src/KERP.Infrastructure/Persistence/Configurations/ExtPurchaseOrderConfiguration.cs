using KERP.Domain.Aggregates.PurchaseOrder;
using KERP.Domain.Aggregates.TempPurchaseOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

public class ExtPurchaseOrderConfiguration
: IEntityTypeConfiguration<ExternalPurchaseOrder>
{
    public void Configure(EntityTypeBuilder<ExternalPurchaseOrder> builder)
    {
        // Krok 1: Definicja nazwy tabeli i schematu
        builder.ToTable("PurchaseOrders");

        // Krok 2: Konfiguracja klucza głównego
        builder.HasKey(x => x.Id);

        // Krok 3: Konfiguracja poszczególnych właściwości (kolumn)
        builder.Property(x => x.PurchaseOrder)
            .IsRequired()
            .HasMaxLength(10); // Przykładowe ograniczenie długości

        builder.Property(x => x.LineNumber)
            .IsRequired();

        builder.Property(x => x.Sequence)
                   .IsRequired();

    }
}
