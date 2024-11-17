using System.Security.Cryptography;
using Tickest.Application.Abstractions.Authentication;

namespace Infrastructure.Authentication;

internal sealed class PasswordHasher : IPasswordHasher
{
    #region Constantes e Algoritmo
    private const int SaltSize = 16;  // Tamanho do salt gerado
    private const int HashSize = 32;  // Tamanho do hash gerado
    private const int Iterations = 500_000; // Número de iterações para derivar a chave
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;  // Algoritmo de hash
    #endregion

    #region Gerar Hash
    /// <summary>
    /// Gera o hash de uma senha.
    /// </summary>
    public string Hash(string password)
    {
        // Gerador de salt e hash
        var salt = RandomNumberGenerator.GetBytes(SaltSize);  // Gera um salt aleatório
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);  // Gera o hash com o salt
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";  // Retorna hash e salt como string concatenada
    }
    #endregion

    #region Verificar Hash
    /// <summary>
    /// Verifica se a senha informada corresponde ao hash armazenado.
    /// </summary>
    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('-');  // Divide hash e salt
        var hash = Convert.FromHexString(parts[0]);  // Converte o hash de volta para byte[]
        var salt = Convert.FromHexString(parts[1]);  // Converte o salt de volta para byte[]
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);  // Gera o hash da senha informada
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);  // Compara hashes de forma segura
    }
    #endregion
}
