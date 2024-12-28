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
    private const int Version = 1; // Versão atual do algoritmo
    #endregion

    #region Gerar Hash
    /// <summary>
    /// Gera o hash de uma senha.
    /// </summary>
    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize); // Gera um salt aleatório
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize); // Gera o hash com o salt

        // Usa interpolação de string para melhor legibilidade (C# 13 suporta diretivas simplificadas)
        return $"{Version}-{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }
    #endregion

    #region Verificar Hash
    /// <summary>
    /// Verifica se a senha informada corresponde ao hash armazenado.
    /// </summary>
    public bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split('-'); // Divide versão, hash e salt
        if (parts.Length != 3)
        {
            throw new FormatException("Formato de hash inválido.");
        }

        // Extrai a versão, hash e salt
        var version = int.Parse(parts[0]);
        if (version != Version)
        {
            throw new InvalidOperationException("Versão do hash incompatível.");
        }

        var hash = Convert.FromHexString(parts[1]);
        var salt = Convert.FromHexString(parts[2]);
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash); // Compara hashes de forma segura
    }
    #endregion

    #region Rehash se Necessário
    /// <summary>
    /// Recalcula o hash se necessário com base na versão atual do algoritmo.
    /// </summary>
    public string? RehashIfNeeded(string password, string passwordHash)
    {
        var parts = passwordHash.Split('-');
        if (parts.Length != 3)
        {
            throw new FormatException("Formato de hash inválido.");
        }

        var version = int.Parse(parts[0]);
        return version < Version ? Hash(password) : null; // Recalcula se a versão for inferior
    }
    #endregion
}
