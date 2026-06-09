using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Configuração da chave primária
        builder.HasKey(d => d.Id);

        // Configuração das propriedades
        builder.Property(d => d.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(d => d.Description)
            .HasMaxLength(500);

        // Relacionamento com o setor
        builder.HasMany(d => d.Sectors) // Relacionamento com setores
            .WithOne(s => s.Department)
            .HasForeignKey(s => s.DepartmentId) // Chave estrangeira no Sector
            .OnDelete(DeleteBehavior.Restrict); // Evita múltiplos caminhos de cascata no SQL Server

        // Relacionamento com o responsável do departamento
        builder.HasOne(d => d.ResponsibleUser)
            .WithMany() // Um usuário pode ser responsável por vários departamentos
            .HasForeignKey(d => d.ResponsibleUserId)
            .OnDelete(DeleteBehavior.SetNull); // Comportamento de exclusão

        // Configuração de tabela
        builder.ToTable("departments");
    }
}
