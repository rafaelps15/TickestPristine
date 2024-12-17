using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider : IPermissionProvider
{
    #region Campos Privados

    private readonly IUserRepository _userRepository;
    private readonly IPermissionRepository _permissionRepository;
    private readonly ILogger<PermissionProvider> _logger;
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions;

    #endregion

    #region Construtor

    public PermissionProvider(
        IUserRepository userRepository,
        IPermissionRepository permissionRepository,
        ILogger<PermissionProvider> logger)
        => (_userRepository, _permissionRepository, _logger, _rolePermissions) =
            (userRepository, permissionRepository, logger, InitializeRolePermissions());

    #endregion

    #region Permissões por Papel

    private static HashSet<string> GetMasterAdminPermissions() => new()
    {
        "FullSystemControl", "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments",
        "ManageAreas", "ManageTickets", "ViewReports", "AccessCriticalSettings"
    };

    private static HashSet<string> GetGeneralAdminPermissions() => new()
    {
        "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments", "ManageAreas",
        "ManageTickets", "ViewReports"
    };

    private static HashSet<string> GetSectorAdminPermissions() => new()
    {
        "ManageSectors", "ManageDepartments", "ManageAreas"
    };

    private static HashSet<string> GetDepartmentAdminPermissions() => new()
    {
        "ManageDepartments", "ManageAreas", "AssignDepartmentRoles"
    };

    private static HashSet<string> GetAreaAdminPermissions() => new()
    {
        "ManageAreas", "ManageTasks", "ManageCollaborators"
    };

    private static HashSet<string> GetTicketManagerPermissions() => new()
    {
        "ManageTickets", "ChangeTicketStatus", "ReassignTickets", "MonitorTicketPerformance"
    };

    private static HashSet<string> GetCollaboratorPermissions() => new()
    {
        "CreateTicket", "TrackTicketStatus", "InteractWithAnalyst"
    };

    private static HashSet<string> GetSupportAnalystPermissions() => new()
    {
        "ManageAssignedTickets", "UpdateTicketStatus", "InteractWithRequester"
    };

    #endregion

    #region Inicialização de Permissões por Papel

    private Dictionary<string, Func<HashSet<string>>> InitializeRolePermissions() =>
        new()
        {
            ["AdminMaster"] = GetMasterAdminPermissions,
            ["GeneralAdmin"] = GetGeneralAdminPermissions,
            ["SectorAdmin"] = GetSectorAdminPermissions,
            ["DepartmentAdmin"] = GetDepartmentAdminPermissions,
            ["AreaAdmin"] = GetAreaAdminPermissions,
            ["TicketManager"] = GetTicketManagerPermissions,
            ["Collaborator"] = GetCollaboratorPermissions,
            ["SupportAnalyst"] = GetSupportAnalystPermissions
        };

    #endregion

    #region Métodos de Permissões

    public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
            throw new TickestException("O ID do usuário não pode ser vazio.", nameof(userId));

        var permissions = new HashSet<string>();

        // Obtém permissões atribuídas diretamente ao usuário
        var userPermissions = await GetPermissionsForUserDirectlyAsync(userId);
        permissions.UnionWith(userPermissions);

        return permissions;
    }

    private Task<IEnumerable<string>> GetPermissionsForUserDirectlyAsync(Guid userId)
    {
        // Implementação do repositório de permissões de usuários
        // Este método será implementado para buscar as permissões atribuídas diretamente ao usuário.
        throw new TickestException();
    }

    public HashSet<string> GetPermissionsForRole(string roleName)
        => _rolePermissions.TryGetValue(roleName, out var permissions)
            ? permissions() // Invoca a função associada à chave
            : new HashSet<string>(); // Retorna um conjunto vazio se o papel não for encontrado

    #endregion

    #region Verificação de Permissão

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permission)
    {
        try
        {
            // Verifica se o usuário tem a permissão
            var userPermissions = await GetPermissionsForUserAsync(userId);
            return userPermissions.Contains(permission);
        }
        catch (TickestException ex)
        {
            _logger.LogError(ex, "Erro ao verificar permissão para o usuário.");
            return false; // Retorna false caso ocorra um erro ao verificar as permissões
        }
    }

    public async Task ValidatePermissionAsync(Guid userId, string permission)
    {
        var hasPermission = await UserHasPermissionAsync(userId, permission);

        if (!hasPermission)
        {
            _logger.LogError("Usuário {UserId} não tem permissão para {Permission}.", userId, permission);
            throw new TickestException($"Usuário não tem permissão para {permission}.");
        }
    }


    #endregion
}