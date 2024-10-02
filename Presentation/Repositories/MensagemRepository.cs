using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class MensagemRepository : BaseRepository<Mensagem>, IMensagemRepository
{
    public MensagemRepository(TickestContext context) : base(context)
    {
    }
}

