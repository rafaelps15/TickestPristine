using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        // Chave primária
        builder.HasKey(rt => rt.Id);

        // Definindo o relacionamento com o User
        builder.HasOne(rt => rt.User) // Um RefreshToken pertence a um User
               .WithMany(u => u.RefreshTokens) // Um User pode ter vários RefreshTokens
               .HasForeignKey(rt => rt.UserId) // A chave estrangeira está na tabela RefreshToken
               .OnDelete(DeleteBehavior.Cascade); // Quando o usuário for deletado, os RefreshTokens são deletados

        // Propriedades
        builder.Property(rt => rt.Token)
               .IsRequired()
               .HasMaxLength(500); // Defina o tamanho conforme necessário

        builder.Property(rt => rt.ExpiresAt)
               .IsRequired();

        builder.Property(rt => rt.IsActive)
               .IsRequired();

        // Definir índice único no Token (se for necessário)
        builder.HasIndex(rt => rt.Token)
               .IsUnique();

        //// Propriedades de auditoria
        //builder.Property(rt => rt.CreatedDate)
        //       .HasDefaultValueSql("GETDATE()");

        //builder.Property(rt => rt.UpdatedDate)
        //       .IsRequired(false);
    }
}
