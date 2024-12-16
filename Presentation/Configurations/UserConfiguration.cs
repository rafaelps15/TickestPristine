using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Configurações básicas da entidade User
        builder.Property(u => u.Name).IsRequired().HasMaxLength(200);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(255);
        builder.Property(u => u.Salt).IsRequired().HasMaxLength(255);

        // Relacionamento N:N com Specialties (via UserSpecialty)
        builder.HasMany(u => u.UserSpecialties)
            .WithOne() // Relacionamento configurado apenas no lado de UserSpecialty
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict); // As especialidades não serão deletadas ao excluir o usuário

        // Relacionamento N:N com Areas e Specialties (via AreaUserSpecialty)
        builder.HasMany(u => u.AreaUserSpecialties)
            .WithOne() // Relacionamento configurado apenas no lado de AreaUserSpecialty
            .HasForeignKey(aus => aus.UserId)
            .OnDelete(DeleteBehavior.Restrict); // As áreas não serão deletadas ao excluir o usuário

        // Relacionamento N:N com Permissions (as permissões serão deletadas ao excluir o usuário)
        builder.HasMany(u => u.Permissions)
            .WithMany() // Sem necessidade de coleções em Permission
            .UsingEntity<Dictionary<string, object>>(
                "UserPermissions",
                j => j.HasOne<Permission>().WithMany().HasForeignKey("PermissionId")
                    .OnDelete(DeleteBehavior.Cascade), // Permissões serão deletadas ao excluir o usuário
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId")
            );

        // Configuração de tabela
        builder.ToTable("Users");
    }
}
