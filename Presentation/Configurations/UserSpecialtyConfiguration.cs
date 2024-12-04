using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class UserSpecialtyConfiguration : IEntityTypeConfiguration<UserSpecialty>
{
    public void Configure(EntityTypeBuilder<UserSpecialty> builder)
    {
        // Chave primária composta (UserId + SpecialtyId)
        builder.HasKey(us => new { us.UserId, us.SpecialtyId });

        // Relacionamento com o User (configuração do lado de UserSpecialty)
        builder.HasOne(us => us.User)
            .WithMany(u => u.UserSpecialties) // Relacionamento no lado de User
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Cascade); // Excluir UserSpecialty ao excluir User

        // Relacionamento com Specialty (configuração no lado de UserSpecialty)
        builder.HasOne(us => us.Specialty)
            .WithMany(s => s.UserSpecialties) // Relacionamento no lado de Specialty
            .HasForeignKey(us => us.SpecialtyId)
            .OnDelete(DeleteBehavior.Cascade); // Excluir UserSpecialty ao excluir Specialty
    }
}
