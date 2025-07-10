using KERP.Domain.Entities.MassUpdate.PurchaseOrder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

public class PurchaseOrderConfiguration : IEntityTypeConfiguration<PurchaseOrderEntity>
{
    public void Configure(EntityTypeBuilder<PurchaseOrderEntity> builder)
    {
        // Mapujemy encję na fikcyjną tabelę z placeholderem _DYNAMIC
        builder.ToTable("PurchaseOrders_DYNAMIC", "bgq");

        // Mówimy EF Core, że ta encja nie ma klucza w tradycyjnym sensie
        // (jest to tabela tylko do odczytu)
        builder.HasNoKey();
    }
}
