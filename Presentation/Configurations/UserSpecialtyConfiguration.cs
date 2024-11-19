using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class UserSpecialtyConfiguration : IEntityTypeConfiguration<UserSpecialty>
{
    public void Configure(EntityTypeBuilder<UserSpecialty> builder)
    {
        builder.ToTable("UserSpecialties");

        // Chave primária composta
        builder.HasKey(us => new { us.UserId, us.SpecialtyId });

        // Configuração para o relacionamento User -> UserSpecialties
        builder.HasOne(us => us.User)
               .WithMany(u => u.UserSpecialties)
               .HasForeignKey(us => us.UserId)
               .HasConstraintName("FK_UserSpecialties_Users_UserId") // Nome explícito para a FK
               .OnDelete(DeleteBehavior.Restrict); // Usar Restrict ou NoAction ao invés de Cascade

        // Configuração para o relacionamento Specialty -> UserSpecialties
        builder.HasOne(us => us.Specialty)
               .WithMany(s => s.UserSpecialties)
               .HasForeignKey(us => us.SpecialtyId)
               .HasConstraintName("FK_UserSpecialties_Specialties_SpecialtyId") // Nome explícito para a FK
               .OnDelete(DeleteBehavior.Restrict); // Usar Restrict ou NoAction ao invés de Cascade
    }

}
