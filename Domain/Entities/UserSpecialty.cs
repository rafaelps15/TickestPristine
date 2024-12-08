﻿using Tickest.Domain.Entities.Base;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities.Specialties;

#region UserSpecialty
/// <summary>
/// UserSpecialty: Representa a relação entre um usuário e uma especialidade.
/// </summary>
public class UserSpecialty : EntityBase
{
    public Guid UserId { get; set; }
    public User User { get; set; }

    public Guid SpecialtyId { get; set; }
    public Specialty Specialty { get; set; }
}
#endregion