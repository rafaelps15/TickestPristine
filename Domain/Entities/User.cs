using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Domain.Entities.Users;

public class User : Entity<Guid>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    public Guid? SectorId { get; set; }
    public Sector Sector { get; set; }

    // Relacionamento N:N com as especialidades
    public ICollection<Specialty> Specialties { get; set; }

    // Relacionamento N:N com as áreas
    public ICollection<Area> Areas { get; set; }

    // Coleção de mensagens enviadas por este usuário
    public ICollection<Message> Messages { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
