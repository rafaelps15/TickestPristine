using System.Security.Cryptography;
using System.Text;

namespace Tickest.Infrastructure.Helpers;

public class PasswordHasher
{
    public static string GenerateSalt(int size = 16)
    {
        // Cria um array de bytes para armazenar o salt
        byte[] saltBytes = new byte[size];

        // Usa RandomNumberGenerator para preencher o array com valores criptograficamente seguros
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }

        // Converte o salt gerado para uma string no formato Base64
        return Convert.ToBase64String(saltBytes);
    }

    public static string HashPassword(string password, string salt, int iterations = 10000, int hashLength = 32)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.", nameof(password));

        if (string.IsNullOrEmpty(salt))
            throw new ArgumentException("Salt cannot be null or empty.", nameof(salt));

        // Converte o salt e a senha para bytes
        byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        // Gera o hash usando PBKDF2
        using (var rfc2898 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
        {
            byte[] hashBytes = rfc2898.GetBytes(hashLength);

            // Converte o hash para string base64
            return Convert.ToBase64String(hashBytes);
        }
    }
}
