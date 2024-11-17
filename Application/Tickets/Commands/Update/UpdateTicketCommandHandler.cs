using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Commands.Update
{
    internal class UpdateTicketCommandHandler : ICommandHandler<UpdateTicketCommand, bool>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IAuthService _authenticator;

        public UpdateTicketCommandHandler(
            ITicketRepository ticketRepository,
            IAuthService authenticator)
        {
            _ticketRepository = ticketRepository;
            _authenticator = authenticator;
        }

        public async Task<bool> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
        {
            var currentUser = await _authenticator.GetCurrentUserAsync();

            return false;

            //USUARIO PODE EDITAR ESSE TICKET????

            //ESSE TICKET PODE SER EDITADO ?????
        }
    }
}
