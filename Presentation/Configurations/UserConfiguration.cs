using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configuração da chave primária
        builder.HasKey(u => u.Id);

        // Configuração dos campos
        builder.Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(u => u.PasswordHash)
            .IsRequired();

        builder.Property(u => u.Salt)
            .IsRequired();

        // Relacionamento com Role (um-para-muitos)
        builder.HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .IsRequired();

        // Relacionamento N:N com Specialties
        builder.HasMany(u => u.Specialties)
            .WithMany(s => s.Users)
            .UsingEntity(j => j.ToTable("UserSpecialties"));

        // Relacionamento de 1:N com Sector
        builder.HasOne(u => u.Sector)  // Relação de 1:N com Setor
            .WithMany()  // Setor pode ter vários usuários
            .HasForeignKey(u => u.SectorId)  // Chave estrangeira
            .OnDelete(DeleteBehavior.SetNull);  // Caso o Setor seja deletado, o campo SectorId ficará nulo

        // Relacionamento N:N com Areas
        builder.HasMany(u => u.Areas)
            .WithMany(a => a.Users)
            .UsingEntity(j => j.ToTable("UserAreas"));


        // Relacionamento de mensagens enviadas por este usuário
        builder.HasMany(u => u.Messages)
            .WithOne(m => m.Sender)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}
