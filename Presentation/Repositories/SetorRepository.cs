using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SetorRepository : BaseRepository<Department>, IDepartmentRepository
{
    public SetorRepository(TickestContext context) : base(context)
    {
    } 
}
