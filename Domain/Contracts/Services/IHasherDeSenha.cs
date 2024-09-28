namespace Tickest.Domain.Contracts.Services;

public interface IHasherDeSenha
{
    string GerarSalt();
    string HashSenha(string senha, string senhaSalt);
}
