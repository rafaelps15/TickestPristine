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
/// <summary>
/// Configuração do relacionamento entre Department e Sector.
///
/// Embora o relacionamento já esteja configurado no lado de Sector, é essencial
/// repetir a configuração no lado de Department para garantir que o Entity Framework Core
/// compreenda corretamente o relacionamento bidirecional entre as entidades.
///
/// Principais razões para configurar explicitamente:
/// 1. **Definição de chave estrangeira**:
///    - Especificar explicitamente a propriedade `SectorId` como chave estrangeira
///      no lado de Department assegura que o EF mapeie corretamente o modelo.
/// 2. **Comportamento de exclusão**:
///    - Configurar o comportamento de exclusão (OnDelete) de forma explícita evita
///      que o EF aplique o comportamento padrão, que pode não ser o desejado
///      (como exclusão em cascata).
/// 3. **Consistência e clareza**:
///    - Repetir a configuração do relacionamento no lado de Department torna o
///      relacionamento consistente e elimina ambiguidades, reduzindo erros potenciais
///      na geração do modelo ou durante execuções.
///
/// Observação:
/// Caso a configuração no lado de Department seja omitida, o EF pode inferir o relacionamento
/// a partir do lado de Sector. No entanto, omissões podem causar comportamentos inesperados,
/// como erros relacionados à exclusão em cascata ou nomes incorretos de colunas no banco de dados.
/// Por essas razões, a configuração explícita é considerada uma boa prática.
/// </summary>
