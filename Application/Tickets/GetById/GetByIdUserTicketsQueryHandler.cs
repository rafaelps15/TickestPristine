using MediatR;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.GetById;

internal sealed class GetByIdUserTicketsQueryHandler(
    IAuthService authService,
    ITicketRepository ticketRepository,
    IPermissionProvider permissionProvider
) : IQueryHandler<GetByIdUserTicketsQuery, List<TicketResponse>>
{
    public async Task<Result<List<TicketResponse>>> Handle(GetByIdUserTicketsQuery query, CancellationToken cancellationToken)
    {
        #region Validação de Permissões

        var currentUser = await authService.GetCurrentUserAsync(cancellationToken);
        await permissionProvider.ValidatePermissionAsync(currentUser, "ViewTickets");

        #endregion

        #region Obtenção dos Tickets

        var tickets = await ticketRepository.GetTicketsByUserAsync(query.UserId, cancellationToken);

        if (tickets == null || !tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        #endregion

        #region Mapeamento para Resposta

        var ticketResponses = tickets
            .Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description
            }).ToList();

        #endregion

        return Result.Success(ticketResponses);
    }
}
