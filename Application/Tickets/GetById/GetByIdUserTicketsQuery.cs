using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Tickets.GetById
{
    public class GetByIdUserTicketsQuery : IQuery<IEnumerable<CompleteTicketResponse>>
    {
        public Guid UserId { get; set; }
    }
}
