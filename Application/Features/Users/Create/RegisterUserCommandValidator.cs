using FluentValidation;

namespace Tickest.Application.Features.Users.Create;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("O email fornecido tem um formato inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");

        RuleFor(x => x.Roles)
           .NotEmpty().WithMessage("Ao menos uma role é obrigatória.")
           .Must(roles => roles.All(role => !string.IsNullOrWhiteSpace(role)))
           .WithMessage("Todas as roles devem ser preenchidas e não podem conter espaços em branco.");
    }
}
