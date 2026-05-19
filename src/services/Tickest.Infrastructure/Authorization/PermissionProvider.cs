using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Constants;
using Tickest.Domain.Exceptions;
using Tickest.Domain.Interfaces.Repositories;

namespace Infrastructure.Authorization;

internal sealed class PermissionProvider : IPermissionProvider
{
    private readonly IUserRepository _userRepository;
    private readonly ILogger<PermissionProvider> _logger;
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions;

    public PermissionProvider(
        IUserRepository userRepository,
        ILogger<PermissionProvider> logger)
        => (_userRepository, _logger, _rolePermissions) =
            (userRepository, logger, InitializeRolePermissions());

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
        "ManageSectors", "ManageDepartments", "ManageAreas", "AccessSystem"
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

    private static Dictionary<string, Func<HashSet<string>>> InitializeRolePermissions() =>
        new()
        {
            [SystemRoles.AdminMaster] = GetMasterAdminPermissions,
            [SystemRoles.GeneralAdmin] = GetGeneralAdminPermissions,
            [SystemRoles.SectorAdmin] = GetSectorAdminPermissions,
            [SystemRoles.DepartmentAdmin] = GetDepartmentAdminPermissions,
            [SystemRoles.AreaAdmin] = GetAreaAdminPermissions,
            [SystemRoles.TicketManager] = GetTicketManagerPermissions,
            [SystemRoles.Collaborator] = GetCollaboratorPermissions,
            [SystemRoles.SupportAnalyst] = GetSupportAnalystPermissions
        };

    public async Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new TickestException("O ID do usuário não pode ser vazio.", nameof(userId));
        }

        var permissions = new HashSet<string>();
        var user = await _userRepository.GetWithPermissionsAsync(userId, CancellationToken.None);

        if (user is null)
        {
            throw new TickestException("Usuário não encontrado.");
        }

        permissions.UnionWith(GetPermissionsForRole(user.Role.Name));
        permissions.UnionWith(user.Permissions.Select(permission => permission.Description));

        return permissions;
    }

    public HashSet<string> GetPermissionsForRole(string roleName)
        => _rolePermissions.TryGetValue(roleName, out var permissions)
            ? permissions()
            : new HashSet<string>();

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permission)
    {
        try
        {
            var userPermissions = await GetPermissionsForUserAsync(userId);
            return userPermissions.Contains(permission);
        }
        catch (TickestException ex)
        {
            _logger.LogError(ex, "Erro ao verificar permissão para o usuário.");
            return false;
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

        _logger.LogInformation("Usuário {UserId} tem permissão para a ação {Permission}.", userId, permission);
    }

    public async Task<bool> CanUserLoginAsync(Guid userId)
    {
        return await UserHasPermissionAsync(userId, "AccessSystem");
    }
}
