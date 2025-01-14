//using System.Security.Cryptography;
//using Tickest.Application.Abstractions.Authentication;
//using Tickest.Domain.Exceptions;

//namespace Tickest.Infrastructure.Authentication;

//internal sealed class PasswordHasher : IPasswordHasher
//{
//    #region Constantes e Algoritmo

//    private const int SaltSize = 16; 
//    private const int HashSize = 32; 
//    private const int Iterations = 500_000; 
//    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;  
//    private const int Version = 1; // Versão atual do algoritmo

//    #endregion

//    #region Gerar Hash

//    /// <summary>
//    /// Gera o hash de uma senha, retornando o hash e o salt.
//    /// </summary>
//    public (string passwordHash, string salt) HashWithSalt(string password)
//    {
//        // Gera um salt aleatório
//        var salt = RandomNumberGenerator.GetBytes(SaltSize);

//        // Gera o hash com o salt usando PBKDF2
//        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

//        // Retorna o hash e salt concatenados de forma legível
//        return (Convert.ToHexString(hash), Convert.ToHexString(salt));
//    }

//    #endregion

//    #region Verificar Hash

//    /// <summary>
//    /// Verifica se a senha informada corresponde ao hash armazenado.
//    /// </summary>
//    public bool Verify(string password, string passwordHash)
//    {
//        var parts = passwordHash.Split('-'); // Divide versão, hash e salt
//        if (parts.Length != 3)
//        {
//            throw new TickestException("Formato de hash inválido.");
//        }

//        // Extrai a versão, hash e salt
//        var version = int.Parse(parts[0]);
//        if (version != Version)
//        {
//            throw new TickestException("Versão do hash incompatível.");
//        }

//        var hash = Convert.FromHexString(parts[1]);
//        var salt = Convert.FromHexString(parts[2]);

//        // Gera o hash da senha de entrada e compara com o hash armazenado
//        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);

//        // Compara hashes de forma segura para evitar ataques de tempo
//        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
//    }

//    #endregion

//    #region Rehash se Necessário

//    /// <summary>
//    /// Recalcula o hash se necessário com base na versão atual do algoritmo.
//    /// </summary>
//    public async Task<string?> RehashIfNeededAsync(string password, string passwordHash)
//    {
//        var parts = passwordHash.Split('-');
//        if (parts.Length != 3)
//        {
//            throw new TickestException("Formato de hash inválido.");
//        }

//        var version = int.Parse(parts[0]);
//        if (version < Version)
//        {
//            var (newPasswordHash, newSalt) = HashWithSalt(password);
//            return $"{Version}-{newPasswordHash}-{newSalt}"; // Concatenando versão, hash e salt 
//        }
//        return null;
//    }

//    #endregion
//}
