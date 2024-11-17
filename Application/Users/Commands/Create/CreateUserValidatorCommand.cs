using FluentValidation;

namespace Tickest.Application.Users.Commands.Create;

public class CreateUserValidatorCommand : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidatorCommand()
    {
        // Validação do nome
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve ter pelo menos 3 caracteres.");

        // Validação do email
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O email é obrigatório.")
            .EmailAddress().WithMessage("Email inválido.");

        // Validação da senha
        RuleFor(x => x.Senha)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
    }
}