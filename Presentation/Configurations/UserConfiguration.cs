using Tickest.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Tickest.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        // Definir o nome da tabela
        builder.ToTable("Users");

        // Configurações de chave primária
        builder.HasKey(u => u.Id);

        // Configuração das propriedades
        builder.Property(u => u.Name)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(100); // Limite de 100 caracteres

        builder.Property(u => u.Email)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(150); // Limite de 150 caracteres

        builder.Property(u => u.PasswordHas)
            .IsRequired() // Campo obrigatório
            .HasMaxLength(200); // Limite de 200 caracteres

        // Configuração de relacionamentos

        // Relacionamento com TicketUser (Muitos para Muitos)
        builder.HasMany(u => u.TicketUsers)
           .WithOne(tu => tu.User) // Cada TicketUser tem um User
           .HasForeignKey(tu => tu.UserId) // Chave estrangeira do User
           .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata

        // Relacionamento com Message (Muitos para Muitos, via Answered)
        builder.HasOne(u => u.Answered) // Relacionamento com a mensagem respondida
            .WithMany(m => m.UsersWhoAnswered) // Coleção de usuários que responderam
            .HasForeignKey(u => u.AnsweredId) // Chave estrangeira que aponta para a mensagem respondida
            .OnDelete(DeleteBehavior.Restrict); // Impede exclusão em cascata acidental

        // Relacionamento com Setor
        builder.HasOne(u => u.UserSector)
            .WithMany(s => s.Users)
            .HasForeignKey(u => u.SectorId)
            .OnDelete(DeleteBehavior.NoAction); // Impede exclusão em cascata no setor

        // Relacionamento com Departamento
        builder.HasOne(u => u.UserDepartment)
            .WithMany(d => d.Users)
            .HasForeignKey(u => u.DepartmentId)
            .OnDelete(DeleteBehavior.NoAction); // Impede exclusão em cascata no departamento

        // Relacionamento com Área
        builder.HasOne(u => u.UserArea)
            .WithMany(a => a.Users)
            .HasForeignKey(u => u.AreaId)
            .OnDelete(DeleteBehavior.NoAction); // Impede exclusão em cascata na área


        // Relacionamento muitos para muitos com Specialty
        builder.HasMany(u => u.UserSpecialties)
            .WithOne(us => us.User)
            .HasForeignKey(us => us.UserId)
            .OnDelete(DeleteBehavior.Restrict); // Impede a exclusão em cascata
    }
}

