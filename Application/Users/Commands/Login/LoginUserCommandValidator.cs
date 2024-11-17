using FluentValidation;
using System.Net.Mail;

namespace Tickest.Application.Users.Commands.Login;

public class LoginUserCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .Must(BeAValidEmail).WithMessage("Email inválido.");

        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Senha é obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
    }

    private static bool BeAValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && new MailAddress(email).Address == email;
}
