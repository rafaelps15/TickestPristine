using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            // Definindo o nome da tabela no banco de dados
            builder.ToTable("UserRoles");

            // Definindo a chave primária composta por UserId e RoleId
            builder.HasKey(ur => new { ur.UserId, ur.RoleId });

            // Configurando o relacionamento entre UserRole e User
            builder.HasOne(ur => ur.User) // Um UserRole tem um User
                .WithMany(u => u.UserRoles) // Um User pode ter muitos UserRoles
                .HasForeignKey(ur => ur.UserId) // Chave estrangeira de User
                .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata ao excluir o UserRole

            // Configurando o relacionamento entre UserRole e Role
            builder.HasOne(ur => ur.Role) // Um UserRole tem um Role
                .WithMany(r => r.UserRoles) // Um Role pode ter muitos UserRoles
                .HasForeignKey(ur => ur.RoleId) // Chave estrangeira de Role
                .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata ao excluir o UserRole
        }
    }
}
