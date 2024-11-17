using MediatR;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Commands.Delete
{
    public class SoftDeleteTicketCommandHandler : ICommandHandler<SoftDeleteTicketCommand, Ticket>
    {
        private readonly IBaseRepository<Ticket> _baseRepository;
        private readonly ILogger<SoftDeleteTicketCommandHandler> _logger;

        public async Task<Ticket> Handle(SoftDeleteTicketCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Iniciando exclusão lógica do ticket: {TicketId}", request.TicketId);

            // Busca o ticket no repositório
            var ticket = await _baseRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
            {
                _logger.LogWarning("Ticket não encontrado: {TicketId}", request.TicketId);
                throw new KeyNotFoundException("Ticket não encontrado.");
            }

            // Atualiza os campos necessários para o soft delete
            ticket.IsDeleted = true;
            ticket.IsActive = false;
            ticket.DeactivatedDate = DateTime.UtcNow;

            await _baseRepository.UpdateAsync(ticket);

            _logger.LogInformation("Ticket excluído logicamente com sucesso: {TicketId}", request.TicketId);
            return ticket;
        }
    }
}
