using FluentValidation;

namespace Tickest.Application.Departments.Create;

public class CreateDepartmentCommandValidator : AbstractValidator<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome do departamento é obrigatório.")
            .MinimumLength(3).WithMessage("O nome do departamento deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("A descrição do departamento é obrigatória.");
    }
}
