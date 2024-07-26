using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Configurations;

public class RegraConfiguration : IEntityTypeConfiguration<Regra>
{
    public void Configure(EntityTypeBuilder<Regra> builder)
    {
        builder.ToTable("TB_REGRA");

        builder.HasKey(p => p.Id);

        builder.HasMany(p => p.UsuarioRegras)
            .WithOne(p => p.Regra)
            .HasForeignKey(p => p.RegraId);
    }
}
