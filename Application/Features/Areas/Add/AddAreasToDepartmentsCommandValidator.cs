using FluentValidation;

namespace Tickest.Application.Features.Areas.Add;

public class AddAreasToDepartmentsCommandValidator : AbstractValidator<AddAreasToDepartmentsCommand>
{
    public AddAreasToDepartmentsCommandValidator()
    {
        RuleFor(command => command.DepartmentId)
            .NotEmpty()
            .WithMessage("O ID do departamento não pode ser vazio.");

        RuleFor(command => command.AreaIds)
            .NotNull()
            .WithMessage("A lista de IDs de áreas não pode ser nula.")
            .Must(areaIds => areaIds != null && areaIds.Any())
            .WithMessage("A lista de IDs de áreas deve conter pelo menos um ID.");
    }
}
