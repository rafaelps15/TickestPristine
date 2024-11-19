using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        // Nome da tabela
        builder.ToTable("Sectors");

        // Chave primária
        builder.HasKey(s => s.Id);

        // Propriedades
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100); // Defina um limite para o nome

        builder.Property(s => s.Description)
            .HasMaxLength(250); // Limite para descrição

        // Relacionamentos
        builder.HasMany(s => s.Departments)
            .WithOne(d => d.Sector)
            .HasForeignKey(d => d.SectorId)
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
    }
}
