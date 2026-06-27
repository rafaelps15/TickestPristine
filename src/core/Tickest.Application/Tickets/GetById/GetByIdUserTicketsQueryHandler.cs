using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Application.Tickets.GetById;

internal sealed class GetByIdUserTicketsQueryHandler(
    IUserContext userContext,
    ITicketRepository ticketRepository)
    : IQueryHandler<GetByIdUserTicketsQuery, List<TicketResponse>>
{
    public async Task<Result<List<TicketResponse>>> Handle(GetByIdUserTicketsQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            throw new TickestException("Usuário inválido.");
        }

        var tickets = await ticketRepository.GetActiveByUserAsync(query.UserId, cancellationToken);

        if (!tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        var response = tickets
            .Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description
            })
            .ToList();

        return Result.Success(response);
    }
}
