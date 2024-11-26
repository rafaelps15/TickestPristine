using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class MessageRepository : GenericRepository<Message>, IMessageRepository
{
    protected readonly TickestContext _context;

    public MessageRepository(TickestContext context) : base(context) =>
        _context = context;

}
