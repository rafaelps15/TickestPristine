using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class MensagemRepository : BaseRepository<Message>, IMensagemRepository
{
    public MensagemRepository(TickestContext context) : base(context)
    {
    }
}

