using KERP.Domain.Entities.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

public class FactoryConfiguration : IEntityTypeConfiguration<FactoryEntity>
{
    public void Configure(EntityTypeBuilder<FactoryEntity> builder)
    {
        builder.ToTable("Factories");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        // --- "ZASIANIE" DANYCH ---
        builder.HasData(
            new FactoryEntity { Id = 241, Name = "Kalmar Stargard" },
            new FactoryEntity { Id = 276, Name = "Kalmar Ottawa" },
            new FactoryEntity { Id = 260, Name = "Kalmar Shanghai" }
        );
    }
}
