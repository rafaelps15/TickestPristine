﻿using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Tickets;

namespace Tickest.Application.Tickets.Delete;

public class SoftDeleteTicketCommand : ICommand<Ticket>
{
    public Guid TicketId { get; set; }
    public Guid UserId { get; set; } // Usuário que está solicitando a exclusão
}

