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
    private readonly IPermissionProvider _permissionProvider;
    private readonly ILogger<ReopenTicketCommandHandler> _logger;

    public ReopenTicketCommandHandler(
        ITicketRepository ticketRepository,
        IUnitOfWork unitOfWork,
        IValidator<ReopenTicketCommand> validator,
        IAuthService authService,
        IPermissionProvider permissionProvider,
        ILogger<ReopenTicketCommandHandler> logger) =>
        (_ticketRepository, _unitOfWork, _validator, _authService, _permissionProvider, _logger) =
        (ticketRepository, unitOfWork, validator, authService, permissionProvider, logger);

    public async Task<Guid> Handle(ReopenTicketCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a reabertura do ticket.");

        #region Validação de Permissões
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            _logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Verificar se o usuário tem permissão para reabrir o ticket
        var hasPermission =  _permissionProvider.UserHasPermissionAsync(currentUser.Id, "ReopenTicket");
        if (!hasPermission)
        {
            _logger.LogError("Usuário não tem permissão para reabrir tickets.");
            throw new TickestException("Usuário não tem permissão para reabrir tickets.");
        }
        #endregion

        #region Obtenção do Ticket
        var ticket = await _ticketRepository.GetByIdAsync(request.TicketId, cancellationToken);
        if (ticket == null)
        {
            _logger.LogError("Ticket não encontrado.");
            throw new TickestException("Ticket não encontrado.");
        }

        if (ticket.IsActive || ticket.IsDeleted)
        {
            _logger.LogError("O ticket já está ativo ou foi deletado.");
            throw new TickestException("O ticket já está ativo ou foi deletado.");
        }

        // Reabre o ticket
        ticket.IsActive = true;
        ticket.Status = TicketStatus.Open;
        #endregion

        #region Persistência no Repositório com UnitOfWork
        await _ticketRepository.UpdateAsync(ticket, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Ticket reaberto com sucesso: {TicketId}", ticket.Id);
        #endregion

        return ticket.Id;
    }
}
