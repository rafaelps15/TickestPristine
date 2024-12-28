using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class RoleRepository : BaseRepository<Role>, IRoleRepository
{
    private readonly TickestContext _context;

    public RoleRepository(TickestContext context) : base(context) =>
        _context = context;

  
}
