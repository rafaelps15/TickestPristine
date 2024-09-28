using System.Security.Cryptography;
using System.Text;
using Tickest.Domain.Contracts.Services;

namespace Tickest.Infrastructure.Helpers;

public class HasherDeSenha : IHasherDeSenha
{
    private const int DefaultIterations = 10000; // Padrão de iterações
    private const int DefaultHashSize = 32; // Constante para o tamanho do hash
    private const int DefaultSaltSize = 16; // Constante para o tamanho do salt

    /// <summary>
    /// Gera um salt criptograficamente seguro com o tamanho especificado.
    /// </summary>
    public string GerarSalt(int tamanho = DefaultSaltSize)
    {
        if (tamanho <= 0)
            throw new ArgumentOutOfRangeException(nameof(tamanho), "O tamanho do salt deve ser maior que zero.");

        // Usando stackalloc para alocar memória na stack em vez do heap
        Span<byte> saltBytes = stackalloc byte[tamanho];
        RandomNumberGenerator.Fill(saltBytes);

        return Convert.ToBase64String(saltBytes);
    }

    /// <summary>
    /// Gera um hash criptografado da senha usando PBKDF2 com o salt fornecido.
    /// </summary>
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
            // Preenche o array temporário com os bytes gerados
            var hash = rfc2898.GetBytes(tamanhoHash);

            // Copia os bytes do hash gerado para o Span<byte> usando CopyTo
            hash.CopyTo(hashBytes);
        }

        // Converte o hashBytes para Base64 e retorna
        return Convert.ToBase64String(hashBytes);
    }
}
