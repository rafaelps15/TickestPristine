using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Tickets.Delete;

internal sealed class SoftDeleteTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    IDateTimeProvider dateTimeProvider,
    ILogger<SoftDeleteTicketCommandHandler> logger)
    : ICommandHandler<SoftDeleteTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SoftDeleteTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando exclusão lógica do ticket: {TicketId}", request.TicketId);

        await permissionProvider.ValidatePermissionAsync(userContext.UserId, SystemPermissions.DeleteTicket);

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);

        if (ticket is null)
        {
            logger.LogWarning("Ticket não encontrado: {TicketId}", request.TicketId);
            throw new TickestException($"Ticket com ID {request.TicketId} não encontrado.");
        }

        if (!ticket.IsActive)
        {
            logger.LogWarning("Ticket já está inativo: {TicketId}", request.TicketId);
            throw new TickestException("O ticket já está inativo.");
        }

        ticket.SoftDelete(dateTimeProvider.UtcNow);

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket excluído logicamente com sucesso: {TicketId}", request.TicketId);

        return Result.Success((Guid)ticket.Id);
    }
}
