using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        // Configuração da chave primária
        builder.HasKey(s => s.Id);

        // Configuração da tabela
        builder.ToTable("Specialties");

        // Propriedades
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(s => s.Description)
            .HasMaxLength(500); // Tamanho máximo para descrição

        // Relacionamento N:N com áreas e usuários é configurado no lado de AreaUserSpecialty
    }
}
