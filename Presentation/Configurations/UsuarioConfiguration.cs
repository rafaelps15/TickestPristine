using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("TB_USUARIO");

        builder.HasKey(p => p.Id);
        builder.Property(p => p.Nome).IsRequired().HasMaxLength(50);
        builder.Property(p => p.Email).IsRequired().HasMaxLength(100);

        builder.HasOne(p => p.Area)
               .WithMany(p => p.Usuarios)
               .HasForeignKey(p => p.AreaId);


        builder.HasMany(u => u.UsuarioRegras)
               .WithOne(ur => ur.Usuario)
               .HasForeignKey(ur => ur.UsuarioId);

        builder.HasMany(u => u.ChamadosSolicitados)
               .WithOne(c => c.Solicitante)
               .HasForeignKey(c => c.SolicitanteId);

        builder.HasMany(u => u.ChamadosAtendentes)
               .WithOne(c => c.Atendente)
               .HasForeignKey(c => c.AtendenteId);

        builder.HasMany(u => u.Mensagens)
               .WithOne(m => m.Usuario)
               .HasForeignKey(m => m.UsuarioId);
    }
}
