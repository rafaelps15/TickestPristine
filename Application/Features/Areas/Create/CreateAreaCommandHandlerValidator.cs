using FluentValidation;

namespace Tickest.Application.Features.Areas.Create;

public class CreateAreaCommandHandlerValidator : AbstractValidator<CreateAreaCommand>
{
    public CreateAreaCommandHandlerValidator()
    {
        // Validação do nome da área
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome da área é obrigatório.")
            .MaximumLength(100).WithMessage("O nome da área não pode ter mais de 100 caracteres.");

        // Validação da descrição da área
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição da área é obrigatória.")
            .MaximumLength(500).WithMessage("A descrição da área não pode ter mais de 500 caracteres.");

        // Validação do gestor da área (se informado)
        RuleFor(x => x.AreaManagerId)
            .Must(id => id == null || id != Guid.Empty).WithMessage("O ID do gestor da área não pode ser nulo ou vazio.");

        // Validar se o setor informado é válido (se necessário)
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O ID do setor é obrigatório.")
            .Must(id => id != Guid.Empty).WithMessage("O ID do setor não pode ser vazio.");

    }
}
