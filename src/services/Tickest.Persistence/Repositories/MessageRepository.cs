using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

internal class MessageRepository : BaseRepository<Message>, IMessageRepository
{
    public MessageRepository(TickestContext context) : base(context) { }
}
