using MediatR;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Contracts.Responses.Reopen;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Commands.Reopen
{
    public interface IUSerService
    {
        Task CreateUser();

        Task UpdateUser();

        Task GetUserById();
    }

    public class ReopenTicketCommandHandler : ICommandHandler<ReopenTicketCommand, ReopenTicketResponse>
    {
        private readonly ITicketRepository _ticketRepository;

        public ReopenTicketCommandHandler(ITicketRepository ticketRepository) =>
            _ticketRepository = ticketRepository;


        public async Task<ReopenTicketResponse> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
        {
            var ticket = await _ticketRepository.GetByIdAsync(request.TicketId);
            if (ticket == null)
            {
                return new ReopenTicketResponse
                {
                    IsReopened = false,
                    Message = "Ticket não encontrado."
                };
            }


            if (ticket.IsActive || ticket.IsDeleted)
            {
                return new ReopenTicketResponse
                {
                    IsReopened = false,
                    Message = "O ticket já está ativo ou foi deletado."
                };
            }

            ticket.IsActive = true;
            ticket.Status = TicketStatus.Open;
            await _ticketRepository.UpdateAsync(ticket);

            return new ReopenTicketResponse
            {
                IsReopened = true,
                Message = "Ticket reaberto com sucesso."
            };
        }
    }
}
