using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Departments;

namespace Tickest.Persistence.Configurations;

public class SectorConfiguration : IEntityTypeConfiguration<Sector>
{
    public void Configure(EntityTypeBuilder<Sector> builder)
    {
        // Configura a chave primária
        builder.HasKey(s => s.Id);

        // Configura o nome do setor, sendo obrigatório e com limite de 100 caracteres
        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        // Configura a descrição do setor, com limite de 500 caracteres
        builder.Property(s => s.Description)
               .HasMaxLength(500);

        // Relacionamento 1:1 com o Gestor do Setor (User)
        builder.HasOne(s => s.SectorManager)  // Um setor tem um gestor
               .WithMany()  // O gestor pode estar associado a vários setores
               .HasForeignKey(s => s.SectorManagerId)  // Chave estrangeira no setor
               .OnDelete(DeleteBehavior.SetNull);  // Se o gestor for excluído, o campo do gestor no setor será nulo

        // Relacionamento 1:N com Departamentos
        builder.HasMany(s => s.Departments)  // Um setor pode ter vários departamentos
               .WithOne(d => d.Sector)  // Cada departamento pertence a um setor
               .HasForeignKey(d => d.SectorId)  // Chave estrangeira no departamento
               .OnDelete(DeleteBehavior.NoAction);  // Evita a exclusão em cascata para evitar ciclos

        // Configuração de tabela
        builder.ToTable("Sectors");
    }
}
