using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Auths;
using Tickest.Domain.Entities.Users;
using Tickest.Persistence.Configurations.Base;

namespace Tickest.Persistence.Configurations;

public class RefreshTokenConfiguration : BaseEntityConfiguration<RefreshToken>
{
    public override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        base.Configure(builder);
        builder.HasOne<User>().WithMany().HasForeignKey(rt => rt.UserId).IsRequired();
        builder.Property(rt => rt.Token).IsRequired().HasMaxLength(500);
        builder.Property(rt => rt.ExpiresAt).IsRequired();
        builder.Property(rt => rt.IsRevoked).IsRequired().HasDefaultValue(false);
        builder.Property(rt => rt.IsUsed).IsRequired().HasDefaultValue(false);
        builder.HasIndex(rt => rt.UserId);
    }
}

