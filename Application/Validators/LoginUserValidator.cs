using FluentValidation;
using System.Net.Mail;
using Tickest.Application.Authentication.Commands.Login;

namespace Tickest.Application.Validators;

public class LoginUserValidator : AbstractValidator<LoginCommand>
{
    public LoginUserValidator()
    {
        //Validação do Email
        RuleFor(x => x.Email)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Email é obrigatório.")
            .Must(BeAValidEmail).WithMessage("Email inválido.");

        //Validação da Senha
        RuleFor(x => x.Password)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Senha é Obrigatória.")
            .MinimumLength(8).WithMessage("A senha deve ter pelo menos 8 caracteres.");
    }

    private static bool BeAValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && new MailAddress(email).Address == email;
}
