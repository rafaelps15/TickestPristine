using System.Net.Mail;
using System.Text.RegularExpressions;
using Tickest.Domain.Exceptions;

namespace Tickest.Application.Users.CriarUsuario;

public class UsuarioValidator : IUsuarioValidator
{
    public void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !MailAddress.TryCreate(email, out _))
            throw new TickestException("Email inválido.");
    }

    public void ValidateSenha(string senha)
    {
        if (!SenhaAtendeCritérios(senha))
            throw new TickestException("A senha deve ter pelo menos 8 caracteres, incluir pelo menos uma letra maiúscula e dois caracteres especiais.");
    }

    private bool SenhaAtendeCritérios(string senha)
    {
        var senhaRegex = new Regex(@"^(?=.*[A-Z])(?=.*[!@#$%^&*()_+\-=\[\]{};':""\\|,.<>\/?]).{8,}$");
        var especialCount = senha.Count(c => "!@#$%^&*()_+-=[]{};':\"|,.<>/?".Contains(c));

        return senhaRegex.IsMatch(senha) && especialCount >= 2;
    }
}
