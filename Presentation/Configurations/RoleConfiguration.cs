using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Define o nome da tabela
        builder.ToTable("TB_ROLE");

        // Chave primária
        builder.HasKey(p => p.Id);

        // Propriedade Name como obrigatória e define o tamanho máximo
        builder.HasMany(p => p.UserRoles)
            .WithOne(p => p.Role)
            .HasForeignKey(p => p.RoleId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.UserRoles)
            .WithOne(p => p.Role)
            .HasForeignKey(p => p.RoleId)
            .OnDelete(DeleteBehavior.Cascade);// Define o comportamento em cascata na exclusão
    }
}
