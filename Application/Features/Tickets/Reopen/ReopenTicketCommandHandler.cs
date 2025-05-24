using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Tickets.Reopen;

internal sealed class ReopenTicketCommandHandler(
    ITicketRepository ticketRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<ReopenTicketCommandHandler> logger)
    : ICommandHandler<ReopenTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a reabertura do ticket.");

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "ReopenTicket");

        logger.LogInformation("Usuário {UserId} autorizado para reabrir o ticket.", currentUser.Id);

        #endregion

        #region Obtenção do Ticket

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId);
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

        // Reabre o ticket
        ticket.IsActive = true;
        ticket.Status = TicketStatus.Open;

        #endregion

        #region Persistência no Repositório

        // Atualiza o ticket no repositório
        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        logger.LogInformation("Ticket reaberto com sucesso: {TicketId}", ticket.Id);

        #endregion

        return ticket.Id;
    }
}
