using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tickest.Domain.Entities;

namespace Tickest.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //builder.HasKey(u => u.Id);
            //builder.Property(u => u.Name).IsRequired().HasMaxLength(100);
            //builder.Property(u => u.Email).IsRequired().HasMaxLength(200);
            //builder.Property(u => u.Password).IsRequired().HasMaxLength(200);
            //builder.Property(u => u.Role).IsRequired().HasMaxLength(100); // Definir o papel
            //builder.HasMany(u => u.Tickets)
            //       .WithOne(t => t.Requester)
            //       .HasForeignKey(t => t.RequesterId); // Relacionamento com Tickets
            //builder.HasMany(u => u.ResponsibleTickets)
            //       .WithOne(t => t.Responsible)
            //       .HasForeignKey(t => t.ResponsibleId); // Relacionamento com Tickets (Responsável)
        }
    }
}