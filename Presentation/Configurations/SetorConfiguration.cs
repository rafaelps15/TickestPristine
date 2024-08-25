using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class SetorConfiguration : IEntityTypeConfiguration<Setor>
{
    public void Configure(EntityTypeBuilder<Setor> builder)
    {
        builder.ToTable("TB_SETOR");

        builder.HasKey(s => s.Id);
        builder.Property(s => s.Nome)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasOne(s => s.Responsavel)
               .WithMany()
               .HasForeignKey(s => s.ResponsavelId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(s => s.Areas)
               .WithOne(a => a.Setor)
               .HasForeignKey(a => a.SetorId);
    }
}
