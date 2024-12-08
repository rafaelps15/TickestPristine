﻿using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.Create;

public record CreateTicketCommand(
     string Title,
     string Description,
     TicketPriority Priority,
     Guid? RequesterId,
     Guid ResponsibleId) : ICommand<Ticket>;

// RequesterId -> Opcional, se um admin criar para outro usuário



