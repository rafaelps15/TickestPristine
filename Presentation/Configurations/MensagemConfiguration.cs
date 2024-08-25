using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class MensagemConfiguration : IEntityTypeConfiguration<Mensagem>
{
    public void Configure(EntityTypeBuilder<Mensagem> builder)
    {
        builder.ToTable("TB_MENSAGEM");

        builder.HasKey(m => m.Id);
        builder.Property(m => m.Conteudo).IsRequired();
        builder.Property(m => m.DataEnvio).IsRequired();

        builder.HasOne(m => m.Usuario)
               .WithMany(u => u.Mensagens)
               .HasForeignKey(m => m.UsuarioId);
    }
}
