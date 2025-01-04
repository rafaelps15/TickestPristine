using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Persistence.Configurations;

public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        // Configura a chave primária
        builder.HasKey(d => d.Id);

        // Configura o nome do departamento, sendo obrigatório e com limite de 100 caracteres
        builder.Property(d => d.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Configura a descrição do departamento, com limite de 500 caracteres
        builder.Property(d => d.Description)
               .HasMaxLength(500);

        // Relacionamento 1:N com o Setor (Setor ao qual o departamento pertence)
        builder.HasOne(d => d.Sector) // Cada departamento pertence a um setor
               .WithMany(s => s.Departments) // Um setor pode ter vários departamentos
               .HasForeignKey(d => d.SectorId) // Chave estrangeira no departamento
               .OnDelete(DeleteBehavior.Cascade); // Se o setor for excluído, os departamentos também serão excluídos

        // Relacionamento 1:1 com o Gestor do Departamento (User)
        builder.HasOne(d => d.DepartmentManager) // O departamento tem um gestor
               .WithMany() // O gestor pode estar associado a vários departamentos
               .HasForeignKey(d => d.DepartmentManagerId) // Chave estrangeira no departamento
               .OnDelete(DeleteBehavior.SetNull); // Se o gestor for excluído, o campo do gestor será nulo

        // Relacionamento 1:N com as Áreas (Um departamento pode ter várias áreas)
        builder.HasMany(d => d.Areas) // Cada departamento pode ter várias áreas
               .WithOne(a => a.Department) // Cada área pertence a um departamento
               .HasForeignKey(a => a.DepartmentId); // Chave estrangeira na área


        // Configuração de tabela
        builder.ToTable("Departments");
    }
}
