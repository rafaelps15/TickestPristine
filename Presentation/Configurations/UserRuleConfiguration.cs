using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class UserRuleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> builder)
    {
        builder.ToTable("TB_USER_RULE");

        builder.HasKey(ur => new { ur.UserId, ur.RuleId });

        builder.HasOne(ur => ur.User)
               .WithMany(u => u.UserRules)
               .HasForeignKey(ur => ur.UserId);

        builder.HasOne(ur => ur.Rule)
               .WithMany(r => r.UserRules)
               .HasForeignKey(ur => ur.RuleId);
    }
}
