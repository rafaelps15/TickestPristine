using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

internal class ChamadoConfiguration : IEntityTypeConfiguration<Chamado>
{
    public void Configure(EntityTypeBuilder<Chamado> builder)
    {
        // Nome da tabela
        builder.ToTable("TB_CHAMADO");

        // Configuração da chave primária
        builder.HasKey(c => c.Id);

        // Configuração das propriedades
        builder.Property(c => c.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(c => c.Descricao)
            .IsRequired()
            .HasMaxLength(1000);

        builder.Property(c => c.DataAbertura)
            .IsRequired();

        builder.Property(c => c.DataFechamento)
            .IsRequired(false);

        builder.Property(c => c.Status)
            .IsRequired();

        builder.Property(c => c.Prioridade)
            .IsRequired();

        // Configuração das relações
        builder.HasOne(c => c.Solicitante)
            .WithMany(u => u.ChamadosSolicitados)
            .HasForeignKey(c => c.SolicitanteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Atendente)
            .WithMany(u => u.ChamadosAtendentes)
            .HasForeignKey(c => c.AtendenteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Analista)
            .WithMany(u => u.ChamadosAtendimentos)
            .HasForeignKey(c => c.AnalistaId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Area)
            .WithMany(a => a.Chamados)
            .HasForeignKey(c => c.AreaId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.Mensagens)
            .WithOne(m => m.Chamado)
            .HasForeignKey(m => m.ChamadoId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
