using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        // Configuração da chave primária
        builder.HasKey(s => s.Id);

        // Configuração das propriedades
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(s => s.Description)
            .HasMaxLength(500);

        // Relacionamento com o responsável do setor
        builder.HasOne(s => s.ResponsibleUser)
            .WithMany() // Um usuário pode ser responsável por vários setores
            .HasForeignKey(s => s.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict); // Comportamento de exclusão

        // Relacionamento com a área (no lado do setor)
        builder.HasMany(s => s.Areas) // Relacionamento com áreas
            .WithOne() // Não precisa de um relacionamento explícito no lado da área
            .HasForeignKey(a => a.SectorId) // Chave estrangeira no lado da área
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata

        // Configuração de tabela
        builder.ToTable("Sectors");
    }
}
