using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Specialties;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Domain.Entities.Users;

public class User : EntityBase
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public string Salt { get; set; }

    // Relacionamento com a entidade Role
    public Guid RoleId { get; set; }
    public Role Role { get; set; }

    // Relacionamento N:N com as especialidades
    public ICollection<Specialty> Specialties { get; set; }

    // Relacionamento N:N com as áreas
    public ICollection<Area> Areas { get; set; }

    // Coleção de mensagens enviadas por este usuário
    public ICollection<Message> Messages { get; set; }

    // Relacionamento muitos-para-muitos com Roles (caso um usuário tenha múltiplos papéis)
    public ICollection<Role> Roles { get; set; }

    // Relacionamento muitos-para-muitos com Permissões
    public ICollection<Permission> Permissions { get; set; }

    public Guid SectorId { get; set; }
    public Sector Sector { get; set; }
}
