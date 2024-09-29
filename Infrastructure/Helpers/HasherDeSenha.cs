using System.Security.Cryptography;
using System.Text;
using Tickest.Domain.Contracts.Services;

namespace Tickest.Infrastructure.Helpers;

public sealed class HasherDeSenha : IHasherDeSenha
{
    private const int DefaultIterations = 10000; // Número padrão de iterações
    private const int DefaultHashSize = 32;      // Tamanho padrão do hash
    private const int DefaultSaltSize = 16;      // Tamanho padrão do salt

    public string GerarSalt(int tamanho = DefaultSaltSize)
    {
        if (tamanho <= 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "O tamanho do salt deve ser maior que zero.");

        Span<byte> saltBytes = stackalloc byte[tamanho];
        RandomNumberGenerator.Fill(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }

    public string HashSenha(string senha, string salt, int iteracoes = DefaultIterations, int tamanhoHash = DefaultHashSize)
    {
        if (string.IsNullOrWhiteSpace(senha))
            throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(senha));

        if (string.IsNullOrWhiteSpace(salt))
            throw new ArgumentException("O salt não pode ser nulo ou vazio.", nameof(salt));

        var saltBytes = Convert.FromBase64String(salt);
        var senhaBytes = Encoding.UTF8.GetBytes(senha);

        Span<byte> hashBytes = stackalloc byte[tamanhoHash];

        using (var rfc2898 = new Rfc2898DeriveBytes(senhaBytes, saltBytes, iteracoes, HashAlgorithmName.SHA256))
        {
            var hash = rfc2898.GetBytes(tamanhoHash);
            hash.CopyTo(hashBytes);
        }

        return Convert.ToBase64String(hashBytes);
    }
}
