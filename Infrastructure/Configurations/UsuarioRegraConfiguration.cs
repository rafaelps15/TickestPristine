using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Configurations;

public class UsuarioRegraConfiguration : IEntityTypeConfiguration<UsuarioRegra>
{
    public void Configure(EntityTypeBuilder<UsuarioRegra> builder)
    {
        builder.ToTable("TB_USUARIO_REGRA");

        builder.HasKey(ur => new { ur.UsuarioId, ur.RegraId });

        builder.HasOne(ur => ur.Usuario)
               .WithMany(u => u.UsuarioRegras)
               .HasForeignKey(ur => ur.UsuarioId);

        builder.HasOne(ur => ur.Regra)
               .WithMany(r => r.UsuarioRegras)
               .HasForeignKey(ur => ur.RegraId);
    }
}
