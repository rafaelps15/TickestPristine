using FluentValidation;

namespace Tickest.Application.Tickets.Create;

public class CreateTicketCommandValidator : AbstractValidator<CreateTicketCommand>
{
    public CreateTicketCommandValidator()
    {
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
}