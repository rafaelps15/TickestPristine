using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Definindo a chave primária (herdada de EntityBase)
        builder.HasKey(r => r.Id);

        // Definindo a propriedade Name como única
        builder.HasIndex(r => r.Name)
            .IsUnique();

        // Definindo o tamanho máximo do nome da role
        builder.Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(256);

        // Relacionamento muitos-para-muitos com Permissions via RolePermissions
        builder.HasMany(r => r.RolePermissions)
            .WithOne(rp => rp.Role)
            .HasForeignKey(rp => rp.RoleId)
            .OnDelete(DeleteBehavior.Cascade); // Exclui RolePermissions ao excluir uma Role

        // Relacionamento 1:N com Users (um papel pode ser atribuído a vários usuários)
        builder.HasMany(r => r.Users)
            .WithOne(u => u.Role) // Um usuário tem um papel
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict); // Não excluir usuários ao excluir um Role
    }
}

