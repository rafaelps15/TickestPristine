using System.Security.Cryptography;
using System.Text;
using Tickest.Domain.Contracts.Services;

namespace Tickest.Infrastructure.Helpers;

public sealed class PasswordHasher : IPasswordHasher
{
    private const int DefaultIterations = 100000; // Número padrão de iterações
    private const int DefaultHashSize = 32;      // Tamanho padrão do hash
    private const int DefaultSaltSize = 16;      // Tamanho padrão do salt

    public string GenerateSalt(int size = DefaultSaltSize)
    {
        if (size <= 0)
            throw new ArgumentOutOfRangeException(nameof(size), "O tamanho do salt deve ser maior que zero.");

        Span<byte> saltBytes = stackalloc byte[size];
        RandomNumberGenerator.Fill(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }

    public string HashPassword(string password, string salt, int iterations = DefaultIterations, int HashSize = DefaultHashSize)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(password));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("O salt não pode ser nulo ou vazio.", nameof(salt));

        var saltBytes = Convert.FromBase64String(salt);
        var passwordBytes = Encoding.UTF8.GetBytes(password);

        Span<byte> hashBytes = stackalloc byte[HashSize];

        using (var rfc2898 = new Rfc2898DeriveBytes(passwordBytes, saltBytes, iterations, HashAlgorithmName.SHA256))
        {
            var hash = rfc2898.GetBytes(HashSize);
            hash.CopyTo(hashBytes);
        }

        return Convert.ToBase64String(hashBytes);
    }
}
