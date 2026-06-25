using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.SharedKernel;
using Tickest.SharedKernel.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.GetById;

internal sealed class GetByIdUserTicketsQueryHandler(
    IUserContext userContext,
    ITicketRepository _ticketRepository
) : IQueryHandler<GetByIdUserTicketsQuery, List<TicketResponse>>
{
    public async Task<Result<List<TicketResponse>>> Handle(GetByIdUserTicketsQuery query, CancellationToken cancellationToken)
    {
        if (query.UserId != userContext.UserId)
        {
            throw new TickestException("Usuário inválido.");
        }

        var tickets = await _ticketRepository.GetActiveByUserAsync(query.UserId, cancellationToken);

        if (tickets == null || !tickets.Any())
        {
            throw new TickestException("Nenhum ticket encontrado para o usuário especificado.");
        }

        // Mapeando os tickets para a resposta
        var ticketResponses = tickets
            .Select(ticket => new TicketResponse
            {
                Id = ticket.Id,
                Title = ticket.Title,
                Status = ticket.Status,
                Description = ticket.Description
            }).ToList();
          
        // Retorna o resultado com os tickets mapeados
        return Result.Success(ticketResponses);
    }
}
