using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Create;

internal sealed class CreateTicketCommandHandler(
    ITicketRepository ticketRepository,
    ILogger<CreateTicketCommandHandler> logger,
    IAuthService authService,
    IPermissionProvider permissionProvider)
    : ICommandHandler<CreateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a criação de um novo ticket.");

        #region Validação de Permissões
        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        var currentUserId = currentUser.Id;

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Verificar se o usuário tem permissão para criar ticket
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUser, "CreateTicket");
        if (!hasPermission)
        {
            logger.LogError("Usuário não tem permissão para criar tickets.");
            throw new TickestException("Usuário não tem permissão para criar tickets.");
        }
        #endregion

        #region Criação do Ticket
        var requesterId = command.RequesterId ?? currentUser.Id;

        var ticket = new Ticket
        {
            Id = Guid.NewGuid(),  
            Title = command.Title,
            Description = command.Description,
            Priority = command.Priority,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow,
            OpenedByUserId = requesterId,
            AssignedToUserId = command.ResponsibleId,
            IsActive = true,
        };

        // Implementação futura do evento de criação de ticket:
        // ticket.Raise(new TicketCreatedDomainEvent(ticket.Id));

        #endregion

        #region Persistência no Repositório
        await ticketRepository.AddAsync(ticket, cancellationToken);
        logger.LogInformation("Ticket criado com sucesso: {TicketId}", ticket.Id);
        #endregion

        return ticket.Id;  
    }
}
