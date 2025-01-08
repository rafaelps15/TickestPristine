using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Entities.Permissions;
using Tickest.Domain.Entities.Users;
using Tickest.Domain.Exceptions;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider : IPermissionProvider
{
    #region Campos Privados

    private readonly ILogger<PermissionProvider> _logger;
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions;

    #endregion

    #region Construtor

    public PermissionProvider(ILogger<PermissionProvider> logger)
        => (_logger, _rolePermissions) =
            (logger, InitializeRolePermissions());

    #endregion

    #region Permissões por Papel

    private static HashSet<string> GetMasterAdminPermissions() => new()
    {
        "FullSystemControl", "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments",
        "ManageAreas", "ManageTickets", "ViewReports", "AccessCriticalSettings", "AccessSystem"
    };

    private static HashSet<string> GetGeneralAdminPermissions() => new()
    {
        "ManageUsers", "ManagePermissions", "ManageSectors", "ManageDepartments", "ManageAreas",
        "ManageTickets", "ViewReports", "AccessSystem"
    };

    private static HashSet<string> GetSectorAdminPermissions() => new()
    {
        "ManageSectors", "ManageDepartments", "ManageAreas", "AccessSystem" ,"CanUserBeResponsible"
    };

    private static HashSet<string> GetDepartmentAdminPermissions() => new()
    {
        "ManageDepartments", "ManageAreas", "AssignDepartmentRoles", "AccessSystem"
    };

    private static HashSet<string> GetAreaAdminPermissions() => new()
    {
        "ManageAreas", "ManageTasks", "ManageCollaborators", "AccessSystem"
    };

    private static HashSet<string> GetTicketManagerPermissions() => new()
    {
        "ManageTickets", "ChangeTicketStatus", "ReassignTickets", "MonitorTicketPerformance", "AccessSystem"
    };

    private static HashSet<string> GetCollaboratorPermissions() => new()
    {
        "CreateTicket", "TrackTicketStatus", "InteractWithAnalyst", "AccessSystem"
    };

    private static HashSet<string> GetSupportAnalystPermissions() => new()
    {
        "ManageAssignedTickets", "UpdateTicketStatus", "InteractWithRequester", "AccessSystem"
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

    public Task<HashSet<string>> GetPermissionsForUserAsync(User user)
    {
        var permissions = new HashSet<string>();

        // Obtém permissões diretamente associadas ao usuário
        permissions.UnionWith(user.Permissions?.Select(p => p.Name) ?? Enumerable.Empty<string>());

        // Se necessário, inclui permissões associadas aos papéis do usuário
        foreach (var role in user.Roles ?? Enumerable.Empty<Role>())
        {
            permissions.UnionWith(role.Permissions?.Select(p => p.Name) ?? Enumerable.Empty<string>());
        }

        return Task.FromResult(permissions);
    }


    public HashSet<string> GetPermissionsForRole(string roleName)
        => _rolePermissions.TryGetValue(roleName, out var permissions)
            ? permissions() // Invoca a função associada à chave
            : new HashSet<string>(); // Retorna um conjunto vazio se o papel não for encontrado

    #endregion

    #region Verificação de Permissão

    public async Task<bool> UserHasPermissionAsync(User user, string permission)
    {
        try
        {
            // Verifica se o usuário tem a permissão
            var userPermissions = await GetPermissionsForUserAsync(user);
            return userPermissions.Contains(permission);
        }
        catch (TickestException ex)
        {
            _logger.LogError(ex, "Erro ao verificar permissão para o usuário.");
            return false; // Retorna false caso ocorra um erro ao verificar as permissões
        }
    }

    public async Task ValidatePermissionAsync(User user, string permission)
    {
        var hasPermission = await UserHasPermissionAsync(user, permission);

        if (!hasPermission)
        {
            _logger.LogError("Usuário {UserId} não tem permissão para {Permission}.", user.Id, permission);
            throw new TickestException($"Usuário não tem permissão para {permission}.");
        }

        _logger.LogInformation("Usuário {UserId} tem permissão para a ação {Permission}.", user.Id, permission);
    }

    #endregion

    // Método que retorna os papéis disponíveis
    public List<string> GetAvailableRoles()
    {
        return _rolePermissions.Keys.ToList();
    }

}
