using FluentValidation;

namespace Tickest.Application.Features.Sectors.Create;

public class CreateSectorCommandValidator : AbstractValidator<CreateSectorCommand>
{
    public CreateSectorCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do setor é obrigatório.")
            .MinimumLength(3).WithMessage("O nome do setor deve ter pelo menos 3 caracteres");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição do setor é obrigatória.");
    }
}
