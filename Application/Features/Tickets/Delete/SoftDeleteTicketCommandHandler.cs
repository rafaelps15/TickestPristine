using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Tickets.Delete;

internal sealed class SoftDeleteTicketCommandHandler(
    ITicketRepository ticketRepository,
    ILogger<SoftDeleteTicketCommandHandler> logger,
    IAuthService authService,
    IPermissionProvider permissionProvider)
    : ICommandHandler<SoftDeleteTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(SoftDeleteTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando exclusão lógica do ticket: {TicketId}", request.TicketId);

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "DeleteTicket");

        logger.LogInformation("Usuário {UserId} autorizado para excluir o ticket.", currentUser.Id);

        #endregion

        #region Validação e manipulação do Ticket

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId);
        if (ticket == null)
        {
            logger.LogWarning("Ticket não encontrado: {TicketId}", request.TicketId);
            throw new TickestException($"Ticket com ID {request.TicketId} não encontrado.");
        }

        // Verifica se o ticket já está inativo
        if (!ticket.IsActive)
        {
            logger.LogWarning("Ticket já está inativo: {TicketId}", request.TicketId);
            throw new TickestException("O ticket já está inativo.");
        }

        ticket.SoftDelete();

        await ticketRepository.UpdateAsync(ticket, cancellationToken);

        #endregion

        logger.LogInformation("Ticket excluído logicamente com sucesso: {TicketId}", request.TicketId);

        return ticket.Id;
    }
}
