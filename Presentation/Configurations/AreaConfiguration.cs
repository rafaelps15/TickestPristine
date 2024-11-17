using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations;

public class EntityConfigurations : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        builder.HasKey(a => a.Id); // Chave primária
        builder.Property(a => a.Name).IsRequired().HasMaxLength(100); // Propriedade obrigatória
        builder.Property(a => a.Description).HasMaxLength(500); // Descrição
        builder.HasOne(a => a.Department) // Relacionamento com o Departamento
               .WithMany(d => d.Areas)
               .HasForeignKey(a => a.DepartmentId);
        builder.HasMany(a => a.Specialties)
               .WithOne(s => s.Area)
               .HasForeignKey(s => s.AreaId); // Relacionamento com Specialties
    }
}