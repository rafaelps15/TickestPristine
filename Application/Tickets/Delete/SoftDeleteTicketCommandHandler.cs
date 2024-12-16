using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Tickets;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Delete;

//verificar esse nome para deixar adequado.
public class SoftDeleteTicketCommandHandler : ICommandHandler<SoftDeleteTicketCommand, Ticket>
{
    private readonly IBaseRepository<Ticket> _genericRepository;
    private readonly ILogger<SoftDeleteTicketCommandHandler> _logger;

    public SoftDeleteTicketCommandHandler(IBaseRepository<Ticket> genericRepository, ILogger<SoftDeleteTicketCommandHandler> logger) =>
        (_genericRepository, _logger) = (genericRepository, logger);

    public async Task<Ticket> Handle(SoftDeleteTicketCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando exclusão lógica do ticket: {TicketId}", request.TicketId);

        // Busca o ticket no repositório
        var ticket = await _genericRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket == null)
        {
            _logger.LogWarning("Ticket não encontrado: {TicketId}", request.TicketId);
            throw new KeyNotFoundException("Ticket não encontrado.");
        }

        // Atualiza os campos necessários para o soft delete
        ticket.IsDeleted = true;
        ticket.IsActive = false;
        ticket.DeactivatedDate = DateTime.UtcNow;

        await _genericRepository.UpdateAsync(ticket, cancellationToken);

        _logger.LogInformation("Ticket excluído logicamente com sucesso: {TicketId}", request.TicketId);
        return ticket;
    }
}
