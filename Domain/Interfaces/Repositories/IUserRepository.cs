using Tickest.Domain.Entities;

namespace Tickest.Domain.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<bool> DoesEmailExistAsync(string email);
    Task<User> ObterUsuarioPorEmailAsync(string email);
    Task<User> ObterUsuarioPorIdAsync(int usuarioId);
    Task<ICollection<UserRole>> ObterRegrasUsuarioAsync(int usuarioId);
}

