using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Reopen;

internal sealed class ReopenTicketCommandHandler(
    ITicketRepository ticketRepository,
    IValidator<ReopenTicketCommand> validator,
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

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Validar permissão do usuário
        await permissionProvider.ValidatePermissionAsync(currentUser.Id, "ReopenTicket");
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
        await ticketRepository.UpdateAsync(ticket, cancellationToken);

        logger.LogInformation("Ticket reaberto com sucesso: {TicketId}", ticket.Id);
        #endregion

        return ticket.Id;
    }

}
