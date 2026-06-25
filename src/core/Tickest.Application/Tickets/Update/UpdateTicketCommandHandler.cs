using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.Domain.Constants;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Update;

internal sealed class UpdateTicketCommandHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository,
    IUnitOfWork unitOfWork,
    IPermissionProvider permissionProvider,
    ILogger<UpdateTicketCommandHandler> logger)
    : ICommandHandler<UpdateTicketCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando a atualização do ticket.");

        var currentUserId = userContext.UserId;

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

        var requiredPermission = ticket.OpenedByUserId == currentUserId
            ? SystemPermissions.UpdateOwnTicket
            : SystemPermissions.ManageTickets;

        await permissionProvider.ValidatePermissionAsync(currentUserId, requiredPermission);

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
