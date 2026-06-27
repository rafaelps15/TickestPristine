using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;

namespace Tickest.Application.Tickets.Create;

internal sealed class CreateTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    ILogger<CreateTicketCommandHandler> logger)
    : ICommandHandler<CreateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo ticket.");

        var currentUserId = userContext.UserId;
        await permissionProvider.ValidatePermissionAsync(currentUserId, SystemPermissions.CreateTicket);

        var ticket = Ticket.Create(
            command.Title,
            command.Description,
            command.Priority,
            command.RequesterId ?? currentUserId,
            command.ResponsibleId,
            command.DepartmentId,
            command.SectorId);

        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket criado com sucesso: {TicketId}", ticket.Id);

        return Result.Success((Guid)ticket.Id);
    }
}
