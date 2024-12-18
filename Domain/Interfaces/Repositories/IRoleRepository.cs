using Tickest.Domain.Entities;
using Tickest.Domain.Entities.Permissions;

namespace Tickest.Domain.Interfaces.Repositories;

/// <summary>
/// Interface para o repositório de roles (papéis) do sistema.
/// Herda de IGenericRepository para operações básicas de CRUD.
/// </summary>
public interface IRoleRepository : IBaseRepository<Role>
{
   
}
