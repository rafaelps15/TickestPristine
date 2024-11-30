using FluentValidation;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Reopen;

public class ReopenTicketCommandHandler : ICommandHandler<ReopenTicketCommand, Guid>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly IValidator<ReopenTicketCommand> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAuthService _authService;
    private readonly ILogger<ReopenTicketCommandHandler> _logger;

    public ReopenTicketCommandHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IValidator<ReopenTicketCommand> validator,
        IAuthService authService,
        ILogger<ReopenTicketCommandHandler> logger) =>
        (_ticketRepository, _unitOfWork, _validator, _authService, _logger) = (ticketRepository, unitOfWork, validator, authService, logger);

    public async Task<Guid> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        ValidateCommand(request);

        // Obtém o usuário atual para verificar permissões ou outras informações
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            throw new TickestException("Usuário não encontrado ou não autenticado.");
        }

        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket == null)
        {
            throw new TickestException("Ticket não encontrado.");
        }

        if (ticket.IsActive || ticket.IsDeleted)
        {
            throw new TickestException("O ticket já está ativo ou foi deletado.");
        }

        // Reabre o ticket
        ticket.IsActive = true;
        ticket.Status = TicketStatus.Open;
        await _ticketRepository.UpdateAsync(ticket, cancellationToken);

        // Salva as alterações no banco de dados
        await _unitOfWork.CommitAsync(cancellationToken);

        // Retorna o ID do ticket reaberto
        return ticket.Id;
    }

    private void ValidateCommand(ReopenTicketCommand request)
    {
        // Valida a prioridade do ticket
        if (!Enum.IsDefined(typeof(TicketPriority), request.Priority))
        {
            _logger.LogWarning("Prioridade inválida fornecida: {Priority}", request.Priority);
            throw new TickestException("A prioridade selecionada é inválida.");
        }

    }
}
