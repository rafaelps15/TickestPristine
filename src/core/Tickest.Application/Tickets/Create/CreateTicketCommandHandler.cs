using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Tickets.Create;

internal sealed class CreateTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateTicketCommandHandler> logger,
    IPermissionProvider permissionProvider)
    : ICommandHandler<CreateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo ticket.");

        var currentUserId = userContext.UserId;
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUserId, SystemPermissions.CreateTicket);

        if (!hasPermission)
        {
            logger.LogError("Usuário não tem permissão para criar tickets.");
            throw new TickestException("Usuário não tem permissão para criar tickets.");
        }

        var requesterId = command.RequesterId ?? currentUserId;

        var ticket = Ticket.Create(
            command.Title,
            command.Description,
            command.Priority,
            requesterId,
            command.ResponsibleId,
            command.DepartmentId,
            command.SectorId);

        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket criado com sucesso: {TicketId}", ticket.Id);

        return ticket.Id;
    }
}
