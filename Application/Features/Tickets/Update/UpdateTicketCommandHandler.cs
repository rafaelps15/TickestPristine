using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Features.Tickets.Update;

internal sealed class UpdateTicketCommandHandler(
    ITicketRepository ticketRepository,
    IAuthService authService,
    IPermissionProvider permissionProvider,
    ILogger<UpdateTicketCommandHandler> logger)
    : ICommandHandler<UpdateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a atualização do ticket.");

        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        var currentUserId = currentUser.Id;

        if (currentUser == null)
        {
            logger.LogError("Usuário não autenticado. Falha ao tentar editar o ticket. Requisição realizada por um usuário não autenticado.");
            throw new TickestException("Usuário não autenticado. A operação de edição do ticket falhou porque o usuário não está autenticado.");
        }

        //Parei aqui.
        var hasPermission = await permissionProvider.UserHasPermissionAsync(currentUser, "EditTicket");
        if (!hasPermission)
        {
            logger.LogWarning("Usuário {UserId} não tem premissão para realizar update no ticket.", currentUserId);
            throw new TickestException("Usuário não tem permissão para update.");
        }

        #endregion

        #region Obtenção de Ticket

        var ticket = await ticketRepository.GetByIdAsync(request.TicketId);

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

        #endregion

        #region Atualização do Ticket

        // Atualizar os dados do ticket com as informações fornecidas na requisição.
        ticket.Description = request.Description;
        ticket.Status = request.Status;

        await ticketRepository.UpdateAsync(ticket, cancellationToken);

        logger.LogInformation("Ticket atualizado com sucesso. Ticket ID: {TicketId}", request.TicketId);

        #endregion

        // Retornar o resultado com o ID do ticket atualizado.
        return Result.Success(ticket.Id);
    }
}
