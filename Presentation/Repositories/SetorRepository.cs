using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class SetorRepository : BaseRepository<Setor>, ISetorRepository
{
    public SetorRepository(TickestContext context) : base(context)
    {
    } 
}
