namespace Tickest.Domain.Contracts.Services;

public interface IHasherDeSenha
{
	string GerarSalt(int tamanho);
	string HashSenha(string senha, string salt, int iteracoes, int tamanhoHas);
}
