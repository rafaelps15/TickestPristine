using FluentValidation;

namespace Tickest.Application.Users.Commands.Delete
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório.")
                .GreaterThan(Guid.Empty).WithMessage("O ID do usuário deve ser válido.");

            RuleFor(x => x.RequestedById)
                .NotEmpty().WithMessage("O ID do solicitante é obrigatório.")
                .GreaterThan(Guid.Empty).WithMessage("O ID do solicitante deve ser válido.");
        }
    }
}
