using FluentValidation;

namespace Tickest.Application.Tickets.Commands.Delete;

public class SoftDeleteTicketCommandValidator : AbstractValidator<SoftDeleteTicketCommand>
{
    public SoftDeleteTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("O ID do ticket é obrigatório.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("O ID do usuário é obrigatório.");
    }
}
