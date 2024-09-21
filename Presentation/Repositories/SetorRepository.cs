using Tickest.Domain.Entities;

namespace Tickest.Persistence.Repositories;

internal class SetorRepository : BaseRepository<Setor>, ISetorRepository
{
    public SetorRepository(TickestContext context) : base(context)
    {
    } 
}
