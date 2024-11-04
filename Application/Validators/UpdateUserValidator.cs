using FluentValidation;
using System.Net.Mail;
using Tickest.Application.Users.Commands.UpdateUserCommand;

namespace Tickest.Application.Validators;

public class UpdateUserValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.UsuerId)
            .NotEmpty().WithMessage("O ID do usuário deve ser informado para a atualização.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .Must(BeAValidEmail).WithMessage("Email inválido.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(100).WithMessage("O nome não pode ter mais de 100 caracteres.");

        RuleFor(x => x.Password)
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password))
            .Must(ContainUpperCase).WithMessage("A senha deve conter pelo menos uma letra maiúscula.")
            .Must(ContainSpecialCharacters).WithMessage("A senha deve conter pelo menos dois caracteres especiais.")
            .When(x => !string.IsNullOrWhiteSpace(x.Password));
    }

    private bool BeAValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        try
        {
            var mailAddress = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    private bool ContainUpperCase(string senha) =>
        senha.Any(char.IsUpper);

    private bool ContainSpecialCharacters(string senha) =>
        senha.Count(c => char.IsPunctuation(c) || char.IsSymbol(c)) >= 2;
}
