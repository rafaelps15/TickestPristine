using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.GetById;

public class GetByIdUserTicketQueryHandler : IQueryHandler<GetByIdUserTicketsQuery, IEnumerable<CompleteTicketResponse>>
{
    private readonly ITicketRepository _ticketRepository;

    public GetByIdUserTicketQueryHandler(ITicketRepository ticketRepository) =>
        _ticketRepository = ticketRepository;

    public async Task<IEnumerable<CompleteTicketResponse>> Handle(GetByIdUserTicketsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new TickestException("Usuário inválido.");
        }

        // Obtendo os tickets do repositório
        var tickets = await _ticketRepository.GetTicketsByUserAsync(request.UserId, cancellationToken);

        // Verifica se o usuário possui tickets
        if (tickets == null || !tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        // Mapeamento de tickets para CompleteTicketResponse
        var ticketResponse = tickets.Select(ticket => new CompleteTicketResponse(
            ticket.Id,
            ticket.Title,
            ticket.Status,
            ticket.Description)
        ).ToList();

        return ticketResponse;
    }
}
