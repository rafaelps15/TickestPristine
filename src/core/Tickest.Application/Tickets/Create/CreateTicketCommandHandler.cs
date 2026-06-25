using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

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

        // Verificar se o usuário tem permissão para criar ticket
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUserId, SystemPermissions.CreateTicket);
        if (!hasPermission)
        {
            logger.LogError("Usuário não tem permissão para criar tickets.");
            throw new TickestException("Usuário não tem permissão para criar tickets.");
        }

        #region Criação do Ticket
        var requesterId = command.RequesterId ?? currentUserId;

        var ticket = new Ticket
        {
            Title = command.Title,
            Description = command.Description,
            Priority = command.Priority,
            Status = TicketStatus.Open,
            OpenedByUserId = requesterId,
            AssignedToUserId = command.ResponsibleId,
            DepartmentId = command.DepartmentId,
            SectorId = command.SectorId
        };

        // Implementação futura do evento de criação de ticket:
        // ticket.Raise(new TicketCreatedDomainEvent(ticket.Id));

        #endregion

        #region Persistência no Repositório
        await ticketRepository.AddAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);
        logger.LogInformation("Ticket criado com sucesso: {TicketId}", ticket.Id);
        #endregion

        return ticket.Id;  
    }
}
