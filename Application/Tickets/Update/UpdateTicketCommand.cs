using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Tickets.Update
{
    public class UpdateTicketCommand : ICommand<bool>
    {
        public Guid TicketId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}
