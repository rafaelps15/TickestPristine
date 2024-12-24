using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.ToTable("RefreshTokens");

        builder.HasKey(rt => rt.Id);

        builder.HasOne<User>()  
            .WithMany() // Muitos refresh tokens para um usuário
            .HasForeignKey(rt => rt.UserId)
            .IsRequired();

        // Configura o campo Token como não nulo
        builder.Property(rt => rt.Token)
            .IsRequired()
            .HasMaxLength(500); 

        // Configura o campo ExpiresAt como não nulo
        builder.Property(rt => rt.ExpiresAt)
            .IsRequired();

        // Configura o campo IsRevoked como não nulo
        builder.Property(rt => rt.IsRevoked)
            .IsRequired()
            .HasDefaultValue(false);

        // Configura o campo IsUsed como não nulo
        builder.Property(rt => rt.IsUsed)
            .IsRequired()
            .HasDefaultValue(false);

        builder.HasIndex(rt => rt.UserId);
    }
}
