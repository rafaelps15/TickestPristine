using FluentValidation;
using System.Linq;
using Tickest.Domain.Enum;

namespace Tickest.Application.Tickets.Update;

public class UpdateTicketCommandValidator : AbstractValidator<UpdateTicketCommand>
{
    public UpdateTicketCommandValidator()
    {
        RuleFor(x => x.TicketId)
            .NotEmpty().WithMessage("TicketId não pode ser vazio.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição não pode ser vazia.")
            .Length(10, 500).WithMessage("A descrição deve ter entre 10 e 500 caracteres.");

        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("O status não pode ser vazio.")
            .Must(BeAValidStatus).WithMessage("O status fornecido não é válido.");
    }

    private bool BeAValidStatus(TicketStatus status)
    {
        // Verificar se o status é um valor válido dentro da enumeração TicketStatus.
        return Enum.IsDefined(typeof(TicketStatus), status);
    }
}
