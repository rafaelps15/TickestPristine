using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Constants;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Reopen;

internal sealed class ReopenTicketCommandHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<ReopenTicketCommandHandler> logger)
    : ICommandHandler<ReopenTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a reabertura do ticket.");

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        await permissionProvider.ValidatePermissionAsync(currentUser.Id, SystemPermissions.ReopenTicket);

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);
        if (ticket == null)
        {
            logger.LogError("Ticket não encontrado.");
            throw new TickestException("Ticket não encontrado.");
        }

        if (ticket.IsActive || ticket.IsDeleted)
        {
            logger.LogError("O ticket já está ativo ou foi deletado.");
            throw new TickestException("O ticket já está ativo ou foi deletado.");
        }

        ticket.Activate();
        ticket.Status = TicketStatus.Open;

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket reaberto com sucesso: {TicketId}", ticket.Id);

        return ticket.Id;
    }
}
