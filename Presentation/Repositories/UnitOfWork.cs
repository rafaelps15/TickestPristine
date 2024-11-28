using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

namespace Tickest.Persistence.Repositories;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    #region - Campos Privados

    private readonly TickestContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IGenericRepository<TickestContext> _genericRepository;
    private bool _disposed;

    #endregion

    #region - Construtor

    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IGenericRepository<TickestContext> genericRepository) =>
        (_context, _userRepository, _roleRepository, _userRoleRepository, _genericRepository) =
        (context, userRepository, roleRepository, userRoleRepository, genericRepository);

    #endregion

    #region - Métodos Públicos

    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class =>
        (IGenericRepository<TEntity>)_genericRepository;

    public async Task<int> CommitAsync()
    {
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            throw new TickestException("Erro ao tentar salvar as alterações no banco de dados.", dbEx);
        }
        catch (Exception ex)
        {
            throw new TickestException("Ocorreu um erro ao processar a transação.", ex);
        }
    }

    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class =>
        query.Where(e => EF.Property<bool>(e, "IsActive"));

    #endregion

    #region - Métodos Privados

    public void Dispose()
    {
        if (!_disposed)
        {
            _context?.Dispose();
            _disposed = true;
        }
        GC.SuppressFinalize(this);
    }

    #endregion

    #region - Propriedades de Repositórios

    public IUserRepository Users => _userRepository;

    public IRoleRepository Roles => _roleRepository;

    public IUserRoleRepository UserRoles => _userRoleRepository;

    public IGenericRepository<TickestContext> GenericRepository => _genericRepository;

    #endregion
}
