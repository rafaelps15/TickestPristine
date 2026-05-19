using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Constants;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Update;

internal sealed class UpdateTicketCommandHandler(
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<UpdateTicketCommandHandler> logger)
    : ICommandHandler<UpdateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a atualização do ticket.");

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado. Falha ao tentar editar o ticket. Requisição realizada por um usuário não autenticado.");
            throw new TickestException("Usuário não autenticado. A operação de edição do ticket falhou porque o usuário não está autenticado.");
        }

        #region Obtenção de Ticket

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);

        if (ticket == null)
        {
            logger.LogError("Ticket não encontrado. Ticket ID: {TicketId}", request.TicketId);
            throw new TickestException("Ticket não encontrado.");
        }

        // Verificar se o ticket está ativo antes de permitir a edição.
        if (!ticket.IsActive)
        {
            logger.LogError("O ticket não está ativo. Ticket ID: {TicketId}", request.TicketId);
            throw new TickestException("O ticket não está ativo e não pode ser editado.");
        }

        var requiredPermission = ticket.OpenedByUserId == currentUser.Id
            ? SystemPermissions.UpdateOwnTicket
            : SystemPermissions.ManageTickets;

        await permissionProvider.ValidatePermissionAsync(currentUser.Id, requiredPermission);

        #endregion

        #region Atualização do Ticket

        // Atualizar os dados do ticket com as informações fornecidas na requisição.
        ticket.Description = request.Description;
        ticket.Status = request.Status;

        await ticketRepository.UpdateAsync(ticket, cancellationToken);
        await unitOfWork.CommitAsync(cancellationToken);

        logger.LogInformation("Ticket atualizado com sucesso. Ticket ID: {TicketId}", request.TicketId);

        #endregion

        // Retornar o resultado com o ID do ticket atualizado.
        return Result<Guid>.Success(ticket.Id);
    }
}
