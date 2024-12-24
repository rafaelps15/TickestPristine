using FluentValidation;

namespace Tickest.Application.Users.Delete;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID do usuário deve ser válido.");

        RuleFor(x => x.RequestedById)
            .NotEmpty().WithMessage("O ID do solicitante é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID do solicitante deve ser válido.");
    }
}
