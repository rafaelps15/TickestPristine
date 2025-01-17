using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        // Definindo a chave primária (herdada de EntityBase)
        builder.HasKey(p => p.Id);

        // Definindo a propriedade Name como única
        builder.HasIndex(p => p.Name)
            .IsUnique();

        // Definindo as propriedades Name e Description
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(512);

        // Relacionamento muitos-para-muitos com RolePermissions
        builder.HasMany(p => p.RolePermissions)
            .WithOne(rp => rp.Permission)
            .HasForeignKey(rp => rp.PermissionId)
            .OnDelete(DeleteBehavior.Cascade); // Exclui RolePermissions ao excluir uma Permission
    }
}
