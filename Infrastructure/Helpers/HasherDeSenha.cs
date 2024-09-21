using System.Security.Cryptography;
using System.Text;

namespace Tickest.Infrastructure.Helpers;

public class HasherDeSenha
{
	public static string GerarSalt(int tamanho = 16)
	{
		byte[] saltBytes = new byte[tamanho];

		using (var rng = RandomNumberGenerator.Create())
		{
			rng.GetBytes(saltBytes);
		}

		return Convert.ToBase64String(saltBytes);
	}

	public static string HashSenha(string senha, string salt, int iteracoes = 10000, int tamanhoHash = 32)
	{
		if (string.IsNullOrEmpty(senha))
			throw new ArgumentException("A senha não pode ser nula ou vazia.", nameof(senha));

		if (string.IsNullOrEmpty(salt))
			throw new ArgumentException("O salt não pode ser nulo ou vazio.", nameof(salt));

		byte[] saltBytes = Encoding.UTF8.GetBytes(salt);
		byte[] senhaBytes = Encoding.UTF8.GetBytes(senha);

		using (var rfc2898 = new Rfc2898DeriveBytes(senhaBytes, saltBytes, iteracoes, HashAlgorithmName.SHA256))
		{
			byte[] hashBytes = rfc2898.GetBytes(tamanhoHash);
			return Convert.ToBase64String(hashBytes);
		}
	}
}
