using FluentValidation;
using System.Net.Mail;
using Tickest.Application.Authentication.Commands.Register;

namespace Tickest.Application.Validators;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .Must(BeAValidEmail).WithMessage("Email inválido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Senha)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .Must(ContainUpperCase).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Must(ContainSpecialCharacters).WithMessage("A senha deve conter pelo menos dois caracteres especiais.");
    }

    private static bool BeAValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && new MailAddress(email).Address == email;

    private static bool ContainUpperCase(string password) =>
        password.Any(char.IsUpper);

    private static bool ContainSpecialCharacters(string password) =>
        password.Count(c => char.IsPunctuation(c) || char.IsSymbol(c)) >= 2;
}
