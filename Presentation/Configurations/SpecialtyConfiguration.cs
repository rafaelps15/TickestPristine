using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        // Nome da tabela
        builder.ToTable("Specialties");

        // Chave primária
        builder.HasKey(s => s.Id);

        // Propriedades
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100); // Limite para o nome

        builder.Property(s => s.Description)
            .HasMaxLength(250); // Limite para a descrição

        // Relacionamentos
        builder.HasOne(s => s.Area)
            .WithMany(a => a.Specialties)
            .HasForeignKey(s => s.AreaId)
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
    }
}
