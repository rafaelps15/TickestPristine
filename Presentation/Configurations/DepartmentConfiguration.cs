using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Definir o nome da tabela
        builder.ToTable("Departments");

        // Definir a chave primária
        builder.HasKey(d => d.Id);

        // Configurações de propriedades
        builder.Property(d => d.Name)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(100); // Limite de 100 caracteres

        builder.Property(d => d.Description)
            .HasMaxLength(250); // Limite de 250 caracteres para a descrição

        // Definir relacionamentos
        builder.HasOne(d => d.Sector) // Relacionamento com o setor
            .WithMany(s => s.Departments) // Um setor pode ter vários departamentos
            .HasForeignKey(d => d.SectorId) // Chave estrangeira
            .OnDelete(DeleteBehavior.Cascade); // Exclusão em cascata
    }
}
