using FluentValidation;

namespace Tickest.Application.Tickets.Reopen;

public class ReopenTicketCommandValidator : AbstractValidator<ReopenTicketCommand>
{
    public ReopenTicketCommandValidator()
    {
        // Validações básicas dos campos do comando
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("O ID do ticket é obrigatório.");
    }
}
