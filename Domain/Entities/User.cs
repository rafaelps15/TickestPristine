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

    public User(string name, string email, string passwordHash, string salt, Guid? sectorId, Sector sector, ICollection<Specialty> specialties, ICollection<Area> areas, ICollection<Message> messages, ICollection<UserRole> userRoles)
    {
        Name = name;
        Email = email;
        PasswordHash = passwordHash;
        Salt = salt;
        SectorId = sectorId;
        Sector = sector;
        Specialties = specialties;
        Areas = areas;
        Messages = messages;
        UserRoles = userRoles;
    }

    public User()
    {

    }
}
