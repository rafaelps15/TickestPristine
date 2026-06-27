using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Tickets.Update;

internal sealed class UpdateTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    ILogger<UpdateTicketCommandHandler> logger)
    : ICommandHandler<UpdateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a atualização do ticket.");

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);

        if (ticket is null)
        {
            logger.LogError("Ticket não encontrado. Ticket ID: {TicketId}", request.TicketId);
            throw new TickestException("Ticket não encontrado.");
        }

        if (!ticket.IsActive)
        {
            logger.LogError("O ticket não está ativo. Ticket ID: {TicketId}", request.TicketId);
            throw new TickestException("O ticket não está ativo e não pode ser editado.");
        }

        var currentUserId = userContext.UserId;
        var requiredPermission = ticket.OpenedByUserId == currentUserId
            ? SystemPermissions.UpdateOwnTicket
            : SystemPermissions.ManageTickets;

        await permissionProvider.ValidatePermissionAsync(currentUserId, requiredPermission);

        ticket.Update(request.Description, request.Status);

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket atualizado com sucesso. Ticket ID: {TicketId}", request.TicketId);

        return Result<Guid>.Success((Guid)ticket.Id);
    }
}
