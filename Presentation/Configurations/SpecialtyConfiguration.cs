using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities.Specialties;

namespace Tickest.Persistence.Configurations;
public class SpecialtyConfiguration : IEntityTypeConfiguration<Specialty>
{
    public void Configure(EntityTypeBuilder<Specialty> builder)
    {
        // Chave primária
        builder.HasKey(s => s.Id);

        // Propriedades
        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(500);  // Ajuste o tamanho máximo conforme necessário

        // Relacionamento com Area (Muitos para Um)
        builder.HasOne(s => s.Area) // Specialty está relacionado com uma Area
            .WithMany(a => a.Specialties) // Uma Area pode ter várias Specialties
            .HasForeignKey(s => s.AreaId) // Chave estrangeira
            .OnDelete(DeleteBehavior.Restrict); // Evita exclusão em cascata

        builder.Property(s => s.Description)
            .IsRequired(false)  // Pode ser opcional
            .HasMaxLength(500);  // Ajuste o tamanho máximo conforme necessário

        // Relacionamento N:N com os usuários
        builder.HasMany(s => s.Users)
            .WithMany(u => u.Specialties)  // Supondo que a classe `User` tenha a propriedade `Specialties`
            .UsingEntity(j => j.ToTable("UserSpecialties"));  // Nome da tabela de junção

        builder.ToTable("Specialties");

    }
}
