using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Persistence.Configurations;

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
    }
}
