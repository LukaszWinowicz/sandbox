using KERP.Domain.Aggregates.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

public class PurchaseOrderReceiptDateChangeRequestConfiguration
: IEntityTypeConfiguration<PurchaseOrderReceiptDateChangeRequest>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderReceiptDateChangeRequest> builder)
    {
        // Krok 1: Definicja nazwy tabeli i schematu
        builder.ToTable("PurchaseOrderReceiptDate", "upd");

        // Krok 2: Konfiguracja klucza głównego
        builder.HasKey(x => x.Id);

        // Krok 3: Konfiguracja poszczególnych właściwości (kolumn)
        builder.Property(x => x.PurchaseOrder)
            .IsRequired()
            .HasMaxLength(10); // Przykładowe ograniczenie długości

        builder.Property(x => x.LineNumber)
            .IsRequired();

        builder.Property(x => x.NewReceiptDate)
            .HasColumnType("datetime2"); // Używamy precyzyjnego typu daty

        builder.Property(x => x.DateType)
            .IsRequired()
            .HasConversion<string>() // Zapisujemy enuma jako string w bazie dla czytelności
            .HasMaxLength(20);

        builder.Property(x => x.UserId)
            .IsRequired();

        // Krok 4: Konfiguracja pól, które nie są mapowane do bazy
        // W naszym przypadku EF Core jest na tyle inteligentny, że sam wykryje,
        // że pole _domainEvents nie powinno być mapowane, ale dla jasności można to zrobić:
        builder.Ignore(x => x.DomainEvents);
    }
}
