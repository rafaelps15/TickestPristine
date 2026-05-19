using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Persistence.Configurations;

public class AreaUserSpecialtyConfiguration : IEntityTypeConfiguration<AreaUserSpecialty>
{
    public void Configure(EntityTypeBuilder<AreaUserSpecialty> builder)
    {
        // Configuração da chave primária composta
        builder.HasKey(aus => new { aus.AreaId, aus.UserId, aus.SpecialtyId });

        // Relacionamento com Area
        builder.HasOne(aus => aus.Area)
            .WithMany(area => area.AreaUserSpecialties)
            .HasForeignKey(aus => aus.AreaId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com User
        builder.HasOne(aus => aus.User)
            .WithMany(user => user.AreaUserSpecialties)
            .HasForeignKey(aus => aus.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relacionamento com Specialty
        builder.HasOne(aus => aus.Specialty)
            .WithMany(specialty => specialty.AreaUserSpecialties)
            .HasForeignKey(aus => aus.SpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        // Configuração de tabela
        builder.ToTable("AreaUserSpecialties");
    }
}
