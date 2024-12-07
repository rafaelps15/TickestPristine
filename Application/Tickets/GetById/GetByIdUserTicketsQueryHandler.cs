using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Common;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Persistence.Data;

namespace Tickest.Application.Tickets.GetById;

internal sealed class GetByIdUserTicketsQueryHandler(
    IAuthService _authService,
    TickestContext _context,
    IUnitOfWork _unitOfWork
) : IQueryHandler<GetByIdUserTicketsQuery, Result<IEnumerable<CompleteTicketResponse>>>
{
    public async Task<Result<IEnumerable<CompleteTicketResponse>>> Handle(GetByIdUserTicketsQuery query, CancellationToken cancellationToken)
    {
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (query.UserId == currentUser.Id)
        {
            throw new TickestException("Usuário inválido.");
        }

        // Obtendo os tickets do repositório
        var tickets = await _unitOfWork.TicketRepository.GetTicketsByUserAsync(query.UserId, cancellationToken);

        if (tickets is null || !tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        var ticketResponses = tickets
            .Select(ticket => new CompleteTicketResponse(
                ticket.Id,
                ticket.Title,
                ticket.Status,
                ticket.Description)
            ).ToList();

        return Result.Success(ticketResponses.AsEnumerable);
    }
}
