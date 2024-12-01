using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Application.Abstractions.Messaging;
using Tickest.Domain.Entities;
using Tickest.Domain.Enum;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Exceptions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tickest.Application.Tickets.Create;

public class CreateTicketCommandHandler : ICommandHandler<CreateTicketCommand, Ticket>
{
    #region Campos Privados
    private readonly ITicketRepository _ticketRepository;
    private readonly ILogger<CreateTicketCommandHandler> _logger;
    private readonly IAuthService _authService;
    private readonly IPermissionProvider _permissionProvider;
    private readonly IUnitOfWork _unitOfWork;
    #endregion

    #region Construtor
    public CreateTicketCommandHandler(
        ITicketRepository ticketRepository,
        ILogger<CreateTicketCommandHandler> logger,
        IAuthService authService,
        IPermissionProvider permissionProvider,
        IUnitOfWork unitOfWork) =>
        (_ticketRepository, _logger, _authService, _permissionProvider, _unitOfWork) =
        (ticketRepository, logger, authService, permissionProvider, unitOfWork);
    #endregion

    #region Manipulação do Comando
    public async Task<Ticket> Handle(CreateTicketCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando a criação de um novo ticket.");

        #region Validação de Permissões
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        if (currentUser == null)
        {
            _logger.LogError("Usuário não autenticado.");
            throw new TickestException("Usuário não autenticado.");
        }

        // Verificar se o usuário tem permissão para criar ticket
        var hasPermission =  _permissionProvider.UserHasPermissionAsync(currentUser.Id, "CreateTicket");
        if (!hasPermission)
        {
            _logger.LogError("Usuário não tem permissão para criar tickets.");
            throw new TickestException("Usuário não tem permissão para criar tickets.");
        }
        #endregion

        #region Criação do Ticket
        var requesterId = command.RequesterId ?? currentUser.Id;

        var ticket = new Ticket
        {
            Title = command.Title,
            Description = command.Description,
            Priority = command.Priority,
            Status = TicketStatus.Open,
            CreatedAt = DateTime.UtcNow,
            RequesterId = requesterId,
            ResponsibleId = command.ResponsibleId,
            IsActive = true,
        };

        // Adicionando permissões para as roles relevantes
        ticket.RolePermissions = new List<TicketRolePermission>
        {
            new TicketRolePermission { Role = "TicketManager", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "Collaborator", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "SupportAnalyst", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "AdminMaster", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "GeneralAdmin", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "SectorAdmin", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "DepartmentAdmin", CanSendMessage = true, CanViewMessage = true },
            new TicketRolePermission { Role = "AreaAdmin", CanSendMessage = true, CanViewMessage = true }
        };
        #endregion

        #region Persistência no Repositório com UnitOfWork
        await _ticketRepository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.CommitAsync(cancellationToken);

        _logger.LogInformation("Ticket criado com sucesso: {TicketId}", ticket.Id);
        #endregion

        return ticket;
    }
    #endregion
}
