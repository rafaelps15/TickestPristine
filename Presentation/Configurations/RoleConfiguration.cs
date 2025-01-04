using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Configuração da chave primária
        builder.HasKey(r => r.Id);

        // Configuração dos campos
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(r => r.Description)
            .HasMaxLength(200);

        builder.Property(r => r.IsActive)
            .HasDefaultValue(true);

        builder.Property(r => r.IsDeleted)
            .HasDefaultValue(false);

        // Relacionamento N:N com Permissions
        builder.HasMany(r => r.Permissions)
            .WithMany(p => p.Roles)
            .UsingEntity(j => j.ToTable("RolePermissions"));

        // Relacionamento um-para-muitos com Users
        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
