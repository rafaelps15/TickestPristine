using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Tickets.Reopen;

internal sealed class ReopenTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    ILogger<ReopenTicketCommandHandler> logger)
    : ICommandHandler<ReopenTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a reabertura do ticket.");

        var currentUserId = userContext.UserId;

        await permissionProvider.ValidatePermissionAsync(currentUserId, SystemPermissions.ReopenTicket);

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);

        if (ticket is null)
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
