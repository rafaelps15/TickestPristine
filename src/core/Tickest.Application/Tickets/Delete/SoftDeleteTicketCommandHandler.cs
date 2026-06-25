using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.Domain.Constants;
using Tickest.Domain.Entities.Tickets;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Delete
{
    internal sealed class SoftDeleteTicketCommandHandler(
        IUserContext userContext,
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        ILogger<SoftDeleteTicketCommandHandler> logger,
        IDateTimeProvider dateTimeProvider,
            IPermissionProvider permissionProvider)
        : ICommandHandler<SoftDeleteTicketCommand, Guid>
    {
        public async Task<Result<Guid>> Handle(SoftDeleteTicketCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Iniciando exclusão lógica do ticket: {TicketId}", request.TicketId);

            #region Validação de Permissões

            var currentuserId = userContext.UserId;

            //Permissão crítica para a continuidade da execução do processo utilizo "ValidatePermissionAsync"
            await permissionProvider.ValidatePermissionAsync(currentuserId, SystemPermissions.DeleteTicket);

            #endregion

            #region Validação e manipulação do Ticket

            // Busca o ticket no repositório após verificar permissões
            var ticket = await ticketRepository.GetByIdAsync(request.TicketId, false, cancellationToken);
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

            // Executa a exclusão lógica
            ticket.SoftDelete(dateTimeProvider.UtcNow);

            // Atualiza o ticket no repositório
            await ticketRepository.UpdateAsync(ticket, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);

            #endregion

            logger.LogInformation("Ticket excluído logicamente com sucesso: {TicketId}", request.TicketId);

            return ticket.Id;
        }
    }
}
