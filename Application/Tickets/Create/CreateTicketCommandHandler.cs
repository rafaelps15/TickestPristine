using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Tickest.Application.Tickets.Create;

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Ticket>
{
    private readonly ITicketRepository _ticketRepository;
    private readonly ILogger<CreateTicketCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IPermissionProvider _permissionProvider;

    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        ILogger<CreateTicketCommandHandler> logger,
        IAuthService authService,
        IPermissionProvider permissionProvider) =>
        (_ticketRepository, _logger, _authService, _permissionProvider) = (ticketRepository, logger, authService, permissionProvider);

    public async Task<Ticket> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a criação de um novo ticket.");

        // Validação de prioridade
        ValidateCommand(request);

        // Verificar se o usuário tem permissão para criar um ticket
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);
        if (currentUser == null)
        {
            throw new UnauthorizedAccessException("Usuário não autenticado.");
        }

        var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(currentUser.Id);
        var requiredPermissions = new HashSet<string> { "CreateTicket", "ManageTickets" };

        // Verifica se o usuário tem todas as permissões necessárias
        if (!requiredPermissions.All(permission => userPermissions.Contains(permission)))
        {
            throw new UnauthorizedAccessException("Usuário não tem permissão para criar um ticket.");
        }

        // Criação do ticket
        var ticket = new Ticket
        {
            Title = request.Title,
            Description = request.Description,
            Priority = request.Priority,
            Status = TicketStatus.Open,
            CreatedDate = DateTime.Now,
            RequesterId = request.RequesterId,
            ResponsibleId = request.ResponsibleId,
            UserId = currentUser.Id,
            IsActive = true,
            IsDeleted = false,
            Messages = new List<Message>(),
            CreatedAt = DateTime.Now
        };

        // Persistência no banco de dados
        await _ticketRepository.AddAsync(ticket, cancellationToken);

        _logger.LogInformation("Ticket criado com sucesso: {Title}", ticket.Title);

        return ticket;
    }

    private void ValidateCommand(CreateTicketCommand request)
    {
        // Validação da prioridade
        if (!Enum.IsDefined(typeof(TicketPriority), request.Priority))
        {
            _logger.LogWarning("Prioridade inválida fornecida: {Priority}", request.Priority);
            throw new TickestException("A prioridade selecionada é inválida.");
        }
    }
}
