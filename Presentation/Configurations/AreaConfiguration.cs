using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;

namespace Tickest.Persistence.Configurations;

public class AreaConfiguration : IEntityTypeConfiguration<Area>
{
    public void Configure(EntityTypeBuilder<Area> builder)
    {
        // Configuração da chave primária (herdada da EntityBase)
        builder.HasKey(a => a.Id);

        // Configuração do nome da área
        builder.Property(a => a.Name)
            .IsRequired()
            .HasMaxLength(100); // Definindo o tamanho máximo para o nome

        // Configuração da descrição da área
        builder.Property(a => a.Description)
            .IsRequired()
            .HasMaxLength(500); // Definindo o tamanho máximo para a descrição

        // Relacionamento N:1 com o Departamento
        builder.HasOne(a => a.Department)
            .WithMany(d => d.Areas)
            .HasForeignKey(a => a.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction); // Impede a exclusão em cascata

        // Relacionamento 1:1 com o Gerente da Área
        builder.HasOne(a => a.AreaManager)
            .WithMany()
            .HasForeignKey(a => a.AreaManagerId)
            .OnDelete(DeleteBehavior.SetNull); // Se o Gerente for excluído, o campo AreaManagerId se tornará NULL

        // Relacionamento N:N com os Usuários (definindo a tabela de junção)
        builder.HasMany(a => a.Users)
            .WithMany(u => u.Areas)
            .UsingEntity<Dictionary<string, object>>(
                "AreaUser", // Nome da tabela de junção
                j => j.HasOne<User>().WithMany().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<Area>().WithMany().HasForeignKey("AreaId").OnDelete(DeleteBehavior.Cascade)
            );
    }
}
