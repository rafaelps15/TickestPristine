﻿using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;

namespace Tickest.Domain.Entities;

public class UserRole 
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public User User { get; set; }
    public Role Role { get; set; }
}
