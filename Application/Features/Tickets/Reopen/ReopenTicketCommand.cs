﻿using Tickest.Application.Abstractions.Messaging;

namespace Tickest.Application.Features.Tickets.Reopen;

public sealed record ReopenTicketCommand(Guid TicketId) : ICommand<Guid>;
