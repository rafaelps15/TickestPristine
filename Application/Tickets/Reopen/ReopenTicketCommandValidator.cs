using FluentValidation;
using Tickest.Application.Abstractions.Authentication;

namespace Tickest.Application.Tickets.Reopen;

public class ReopenTicketCommandValidator : AbstractValidator<ReopenTicketCommand>
{
    private readonly IPermissionProvider _permissionProvider;
    private readonly IAuthService _authService;

    public ReopenTicketCommandValidator(IPermissionProvider permissionProvider, IAuthService authService) =>
        (_permissionProvider , _authService) = (permissionProvider, authService);

    public ReopenTicketCommandValidator()
    {
        // Validação das permissões para reabertura
        RuleFor(x => x)
            .MustAsync(ValidateUserPermissionsAsync)
            .WithMessage("Usuário não tem permissão para reabrir o ticket.");
    }

    private async Task<bool> ValidateUserPermissionsAsync(ReopenTicketCommand command, CancellationToken token)
    {
        var currentUser = await _authService.GetCurrentUserAsync(token);

        if (currentUser == null)
        {
            return false; 
        }

        var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(currentUser.Id);
        var requiredPermissions = new HashSet<string> { "ReopenTicket", "ManageTickets" };

        // Verifica se o usuário tem todas as permissões necessárias
        return requiredPermissions.All(permission => userPermissions.Contains(permission));
    }
}

