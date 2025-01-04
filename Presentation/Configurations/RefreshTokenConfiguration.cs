using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Definindo a chave primária
        builder.HasKey(rt => rt.Id);

        // Propriedades
        builder.Property(rt => rt.UserId)
            .IsRequired();

        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500);  // Ajuste o tamanho do Token conforme necessário

        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);  // Valor padrão para "não revogado"

        builder.Property(rt => rt.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);  // Valor padrão para "não usado"

        // Relacionamento: Um RefreshToken pertence a um Usuário
        builder.HasOne<User>()  // Supondo que haja uma classe Usuario no seu domínio
            .WithMany()  // Um usuário pode ter vários tokens de refresh
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);  // Excluir tokens ao excluir o usuário

        builder.ToTable("RefreshTokens");


        // Outras validações e regras de negócios podem ser aplicadas aqui, conforme necessário
    }
}
