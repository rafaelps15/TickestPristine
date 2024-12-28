namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface responsável pela geração, verificação e rehashing de senhas.
/// Fornece métodos para criar hashes de senhas com sal, validar senhas informadas
/// e atualizar hashes quando a versão do algoritmo de hash for alterada.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Gera um hash de senha com salt, retornando o hash e o salt gerado.
    /// </summary>
    /// <param name="password">A senha a ser gerada.</param>
    /// <returns>Um tuplo contendo o hash gerado e o salt utilizado.</returns>
    (string passwordHash, string salt) HashWithSalt(string password);

    /// <summary>
    /// Verifica se a senha informada corresponde ao hash armazenado.
    /// </summary>
    /// <param name="password">A senha a ser verificada.</param>
    /// <param name="passwordHash">O hash da senha armazenado.</param>
    /// <returns>True se a senha corresponder ao hash, caso contrário, False.</returns>
    bool Verify(string password, string passwordHash);

    /// <summary>
    /// Recalcula o hash da senha se necessário, caso a versão do algoritmo de hash tenha mudado.
    /// </summary>
    /// <param name="password">A senha para rehashing.</param>
    /// <param name="passwordHash">O hash de senha atual a ser verificado.</param>
    /// <returns>Um novo hash concatenado com a versão e o salt, ou null se não for necessário rehashing.</returns>
    Task<string?> RehashIfNeededAsync(string password, string passwordHash);
}
