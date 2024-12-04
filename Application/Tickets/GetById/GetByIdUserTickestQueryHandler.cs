using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;

namespace Tickest.Application.Tickets.GetById;

public class GetByIdUserTicketQueryHandler : IQueryHandler<GetByIdUserTicketsQuery, IEnumerable<CompleteTicketResponse>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByIdUserTicketQueryHandler(IUnitOfWork unitOfWork) =>
        _unitOfWork = unitOfWork;

    public async Task<IEnumerable<CompleteTicketResponse>> Handle(GetByIdUserTicketsQuery request, CancellationToken cancellationToken)
    {
        if (request.UserId == Guid.Empty)
        {
            throw new TickestException("Usuário inválido.");
        }

        // Obtendo os tickets do repositório com o filtro IsActive
        var tickets = await _unitOfWork.TicketRepository.GetTicketsByUserAsync(request.UserId, cancellationToken);

        if (tickets == null || !tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        var ticketResponse = tickets
            .Where(ticket => ticket.IsActive)  // Filtro para garantir que apenas tickets ativos sejam retornados
            .Select(ticket => new CompleteTicketResponse(
                ticket.Id,
                ticket.Title,
                ticket.Status,
                ticket.Description)
            ).ToList();

        return ticketResponse;
    }
}
