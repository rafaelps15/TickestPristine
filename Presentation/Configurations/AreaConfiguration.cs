using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Definir o nome da tabela
        builder.ToTable("Areas");

        // Definir a chave primária
        builder.HasKey(a => a.Id);

        // Configurações de propriedades
        builder.Property(a => a.Name)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(100); // Limite de 100 caracteres

        builder.Property(a => a.Description)
            .HasMaxLength(250); // Limite de 250 caracteres para a descrição

        // Definir relacionamentos
        builder.HasOne(a => a.Department) // Relacionamento com Departamento
            .WithMany(d => d.Areas) // Um departamento pode ter várias áreas
            .HasForeignKey(a => a.DepartmentId) // Chave estrangeira
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
    }
}
