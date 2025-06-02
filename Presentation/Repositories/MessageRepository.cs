using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class MessageRepository : BaseRepository<Message,Guid>, IMessageRepository
{
    protected readonly TickestContext _context;

    public MessageRepository(TickestContext context) : base(context) =>
        _context = context;

}
