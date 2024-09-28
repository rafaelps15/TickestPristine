namespace Tickest.Application.Users.CriarUsuario;

/// <summary>
/// Interface responsável pela validação dos dados do usuário.
/// Define métodos para validar o formato do email e os critérios da senha.
/// </summary>
public interface IUsuarioValidator
{
    /// <summary>
    /// Valida o formato do email.
    /// Lança uma exceção se o email estiver em branco ou não for válido.
    /// </summary>
    /// <param name="email">O email a ser validado.</param>
    void ValidateEmail(string email);

    /// <summary>
    /// Valida a senha de acordo com critérios específicos.
    /// Lança uma exceção se a senha não atender aos critérios definidos.
    /// </summary>
    /// <param name="senha">A senha a ser validada.</param>
    void ValidateSenha(string senha);
}
