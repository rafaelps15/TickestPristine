using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Configuração da chave primária
        builder.HasKey(a => a.Id);

        // Configuração das propriedades
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(a => a.Description)
            .HasMaxLength(500);

        // Relacionamento com o responsável pela área
        builder.HasOne(a => a.ResponsibleUser)
            .WithMany() // Um usuário pode ser responsável por várias áreas
            .HasForeignKey(a => a.ResponsibleUserId)
            .OnDelete(DeleteBehavior.Restrict); // Comportamento de exclusão

        // Relação N:N com usuários e especialidades
        builder.HasMany(a => a.AreaUserSpecialties)
            .WithOne() // Relacionamento sem a necessidade de um lado explícito em AreaUserSpecialty
            .HasForeignKey(aus => aus.AreaId)
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata

        // Configuração de tabela
        builder.ToTable("Areas");
    }
}
