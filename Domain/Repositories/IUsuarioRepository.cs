using Tickest.Domain.Entities;

namespace Tickest.Domain.Repositories;

public interface IUsuarioRepository : IBaseRepotirory<Usuario>
{
    Task<bool> ExisteUsuarioEmailAsync(string email);

    Task<Usuario> GetByEmailAsync(string email);
}

