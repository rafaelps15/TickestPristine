using Tickest.Domain.Entities;

namespace Tickest.Persistence.Repositories;

internal class MensagemRepository : BaseRepository<Mensagem>, IMensagemRepository
{
    public MensagemRepository(TickestContext context) : base(context)
    {
    }
}

