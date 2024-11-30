using FluentValidation;
using Tickest.Application.Abstractions.Authentication;

namespace Tickest.Application.Tickets.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    private readonly IPermissionProvider _permissionProvider;
    private readonly IAuthService _authService;

    public CreateTicketCommandValidator(IPermissionProvider permissionProvider, IAuthService authService) =>
        (_permissionProvider, _authService) = (permissionProvider, authService);

    public CreateTicketCommandValidator()
    {
        // Validação das permissões para criação do ticket
        RuleFor(x => x)
            .MustAsync(ValidateUserPermissionsAsync)
            .WithMessage("Usuário não tem permissão para criar um ticket.");

        // Validações dos campos do ticket
        RuleFor(x => x.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("O título é obrigatório.")
            .MaximumLength(100).WithMessage("O título não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Description)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(500).WithMessage("A descrição não pode ter mais de 500 caracteres.");

        RuleFor(x => x.Priority)
            .IsInEnum().WithMessage("A prioridade selecionada é inválida.");

        RuleFor(x => x.RequesterId)
            .NotEmpty().WithMessage("O ID do solicitante é obrigatório.");

        RuleFor(x => x.ResponsibleId)
            .NotEmpty().WithMessage("O ID do responsável é obrigatório.");
    }

    private async Task<bool> ValidateUserPermissionsAsync(CreateTicketCommand command, CancellationToken token)
    {
        var currentUser = await _authService.GetCurrentUserAsync(token);

        if (currentUser == null)
        {
            return false;
        }

        var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(currentUser.Id);
        var requiredPermissions = new HashSet<string> { "CreateTicket" };

        // Verifica se o usuário tem todas as permissões necessárias
        return requiredPermissions.All(permission => userPermissions.Contains(permission));
    }
}
