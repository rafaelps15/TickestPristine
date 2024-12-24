using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class AreaUserSpecialtyConfiguration : IEntityTypeConfiguration<AreaUserSpecialty>
{
    public void Configure(EntityTypeBuilder<AreaUserSpecialty> builder)
    {
        // Configuração da chave primária composta
        builder.HasKey(aus => new { aus.AreaId, aus.UserId, aus.SpecialtyId });

        // Relacionamento com Area
        builder.HasOne(aus => aus.Area)
            .WithMany() // Sem necessidade de configuração explícita de coleções de áreas na área
            .HasForeignKey(aus => aus.AreaId)
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata

        // Relacionamento com User
        builder.HasOne(aus => aus.User)
            .WithMany() // Sem necessidade de configuração explícita de coleções de usuários no usuário
            .HasForeignKey(aus => aus.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de exclusão para evitar deleção de usuários

        // Relacionamento com Specialty
        builder.HasOne(aus => aus.Specialty)
            .WithMany() // Sem necessidade de configuração explícita de coleções de especialidades na especialidade
            .HasForeignKey(aus => aus.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict); // Restrição de exclusão para evitar deleção de especialidades

        // Configuração de tabela
        builder.ToTable("AreaUserSpecialties");
    }
}
