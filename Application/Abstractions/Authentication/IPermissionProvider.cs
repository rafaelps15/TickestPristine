namespace Tickest.Application.Abstractions.Authentication;

/// <summary>
/// Interface responsável por fornecer permissões para usuários e papéis.
/// </summary>
public interface IPermissionProvider
{
    /// <summary>
    /// Verifica se o usuário tem permissão para acessar o sistema.
    /// </summary>
    /// <param name="userId">O ID do usuário.</param>
    /// <returns>Retorna true se o usuário tiver permissão para acessar o sistema, caso contrário, false.</returns>
    Task<bool> CanUserLoginAsync(Guid userId);

    /// <summary>
    /// Obtém as permissões atribuídas a um usuário específico.
    /// Isso pode incluir permissões diretamente atribuídas ou permissões herdadas de papéis.
    /// </summary>
    /// <param name="userId">Identificador único do usuário.</param>
    /// <returns>Conjunto de permissões atribuídas ao usuário.</returns>
    /// <exception cref="ArgumentException">Lançada se o <paramref name="userId"/> for inválido.</exception>
    Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId);

    /// <summary>
    /// Obtém as permissões associadas a um papel específico.
    /// </summary>
    /// <param name="roleName">Nome do papel para o qual as permissões são recuperadas.</param>
    /// <returns>Conjunto de permissões associadas ao papel.</returns>
    /// <exception cref="ArgumentException">Lançada se o <paramref name="roleName"/> for inválido.</exception>
    HashSet<string> GetPermissionsForRole(string roleName);

    /// <summary>
    /// Verifica se um usuário possui uma permissão específica.
    /// </summary>
    /// <param name="userId">Identificador único do usuário.</param>
    /// <param name="permission">A permissão a ser verificada.</param>
    /// <returns>Retorna true se o usuário tiver a permissão; caso contrário, false.</returns>
    /// <exception cref="ArgumentException">Lançada se o <paramref name="userId"/> ou <paramref name="permission"/> for inválido.</exception>
    Task<bool> UserHasPermissionAsync(Guid userId, string permission);

    /// <summary>
    /// Valida se um usuário possui uma permissão específica.
    /// Lança uma exceção caso o usuário não tenha a permissão necessária.
    /// </summary>
    /// <param name="userId">Identificador único do usuário.</param>
    /// <param name="permission">A permissão a ser verificada.</param>
    /// <exception cref="TickestException">Lançada se o usuário não tiver a permissão.</exception>
    Task ValidatePermissionAsync(Guid userId, string permission);
}
