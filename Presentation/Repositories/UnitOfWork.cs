using Microsoft.EntityFrameworkCore;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.Persistence.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly TickestContext _context;
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRoleRepository _userRoleRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IGenericRepository<TickestContext> _genericRepository;
    private readonly ITicketRepository _ticketRepository;
    private bool _disposed;

    public UnitOfWork(
        TickestContext context,
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        IUserRoleRepository userRoleRepository,
        IRefreshTokenRepository refreshTokenRepository,
        ITicketRepository ticketRepository,
        IGenericRepository<TickestContext> genericRepository) =>
        (_context, _userRepository, _roleRepository, _userRoleRepository, _refreshTokenRepository, _ticketRepository, _genericRepository) =
        (context, userRepository, roleRepository, userRoleRepository, refreshTokenRepository, ticketRepository, genericRepository);

    #region - Métodos Públicos

    public IUserRepository Users => _userRepository;
    public IRoleRepository Roles => _roleRepository;
    public IUserRoleRepository UserRoles => _userRoleRepository;
    public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository;
    public ITicketRepository TicketRepository => _ticketRepository;


    public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class =>
        (IGenericRepository<TEntity>)_genericRepository;

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
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

    /// <summary>
    /// Aplica o filtro para considerar apenas entidades ativas.
    /// </summary>
    /// <typeparam name="TEntity">O tipo da entidade.</typeparam>
    /// <param name="query">A consulta que será filtrada.</param>
    /// <returns>A consulta filtrada para retornar apenas entidades ativas.</returns>
    public IQueryable<TEntity> ApplyFilters<TEntity>(IQueryable<TEntity> query) where TEntity : class
    {
        // Exemplo de filtro que pode ser aplicado
        return query.Where(entity => EF.Property<bool>(entity, "IsActive"));
    }

    /// <summary>
    /// Libera os recursos utilizados pela unidade de trabalho.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Libera os recursos utilizados pela unidade de trabalho.
    /// </summary>
    /// <param name="disposing">Indica se o método foi chamado pelo Dispose() ou pelo finalizador.</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            _disposed = true;
        }
    }

    #endregion
}
