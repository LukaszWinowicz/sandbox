using KERP.Domain.Aggregates.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KERP.Infrastructure.Persistence.Configurations;

/// <summary>
/// Konfiguracja mapowania encji Factory do bazy danych.
/// </summary>
public class FactoryConfiguration : IEntityTypeConfiguration<Factory>
{
    public void Configure(EntityTypeBuilder<Factory> builder)
    {
        builder.ToTable("Factories");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FactoryId)
            .IsRequired();

        builder.Property(f => f.Description)
            .IsRequired()
            .HasMaxLength(50);

        // Indeks na FactoryId dla szybkiego wyszukiwania
        builder.HasIndex(f => f.FactoryId)
            .IsUnique();
    }
}