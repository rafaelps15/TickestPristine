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
            .HasMaxLength(200);  // Ajuste o tamanho máximo conforme necessário

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
