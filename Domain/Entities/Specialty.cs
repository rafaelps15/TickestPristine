﻿using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Sectors;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Specialties;

#region Specialty
/// <summary>
/// Specialty: Representa uma especialidade dentro de uma área, indicando uma área de especialização específica.
/// </summary>
public class Specialty : Entity<Guid>
{
    public string Name { get; set; } 
    public string Description { get; set; }

    public List<User> Users { get; set; }  // Relacionamento N:N com os Usuários (um usuário pode ter várias especialidades)
    public Guid AreaId { get; set; }
    public Area Area { get; set; }
}
#endregion