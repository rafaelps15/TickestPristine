using Tickest.Domain.Entities;

namespace Tickest.Application.Abstractions.Authentication
{
    public interface ITokenProvider
    {
        /// <summary>
        /// Cria um token JWT para o usuário.
        /// </summary>
        /// <param name="user">Usuário para quem o token será gerado.</param>
        /// <returns>Token JWT gerado.</returns>
        string Create(User user, double expirationInMinutes);
    }
}
