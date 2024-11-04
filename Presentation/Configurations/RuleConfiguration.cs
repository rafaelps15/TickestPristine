using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class RuleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Define o nome da tabela
        builder.ToTable("TB_RULE");

        // Chave primária
        builder.HasKey(p => p.Id);

        // Propriedade Name como obrigatória e define o tamanho máximo
        builder.HasMany(p => p.UserRules)
            .WithOne(p => p.Rule)
            .HasForeignKey(p => p.RuleId);

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.HasMany(p => p.UserRules)
            .WithOne(p => p.Rule)
            .HasForeignKey(p => p.RuleId)
            .OnDelete(DeleteBehavior.Cascade);// Define o comportamento em cascata na exclusão
    }
}
