using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Entities;
using Tickest.Domain.Repositories;
using Tickest.Persistence;

namespace Tickest.Persistence.Repositories;

internal class MensagemRepository : BaseRepository<Mensagem>, IMensagemRepository
{
    public MensagemRepository(TickestContext context) : base(context)
    {
    }
}

