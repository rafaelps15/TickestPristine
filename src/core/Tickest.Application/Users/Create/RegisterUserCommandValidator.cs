using FluentValidation;

namespace Tickest.Application.Users.Create;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.EmployeeCode)
            .NotEmpty().WithMessage("O código do funcionário é obrigatório.")
            .MaximumLength(10).WithMessage("O código do funcionário deve ter no máximo 10 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email fornecido tem um formato inválido.")
            .Must(email => string.IsNullOrWhiteSpace(email) || email == email.ToLowerInvariant())
            .WithMessage("O email deve estar em letras minúsculas.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");

        // Temporario para teste: reativar quando setor e especialidades forem obrigatorios no cadastro.
        // RuleFor(x => x.SectorId)
        //     .NotEmpty().WithMessage("O setor é obrigatório.");

        // RuleFor(x => x.SpecialtyIds)
        //     .NotEmpty().WithMessage("Informe pelo menos uma especialidade.");
    }
}
