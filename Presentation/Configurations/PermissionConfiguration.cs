using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Configuração da chave primária
        builder.HasKey(p => p.Id);

        // Configuração dos campos
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(p => p.Description)
            .HasMaxLength(200);

        // Relacionamento N:N com Roles
        builder.HasMany(p => p.Roles)
            .WithMany(r => r.Permissions)
            .UsingEntity(j => j.ToTable("RolePermissions"));

        // Relacionamento N:N com Users
        builder.HasMany(p => p.Users)
            .WithMany(u => u.Permissions)
            .UsingEntity(j => j.ToTable("UserPermissions"));
    }
}
