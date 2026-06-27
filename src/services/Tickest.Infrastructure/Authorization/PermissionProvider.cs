using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;
using Tickest.Domain.Constants;
using Tickest.Domain.Interfaces.Repositories;
using Tickest.SharedKernel.Exceptions;

namespace Tickest.Infrastructure.Authorization;

internal sealed class PermissionProvider(
    IUserRepository userRepository,
    ILogger<PermissionProvider> logger)
    : IPermissionProvider
{
    private readonly Dictionary<string, Func<HashSet<string>>> _rolePermissions = InitializeRolePermissions();

    public async Task<HashSet<string>> GetForUserIdAsync(Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new TickestException("O ID do usuário não pode ser vazio.", nameof(userId));
        }

        var user = await userRepository.GetWithPermissionsAsync(userId, CancellationToken.None);

        if (user is null)
        {
            throw new TickestException("Usuário não encontrado.");
        }

        var permissions = GetPermissionsForRole(user.Role.Name);
        permissions.UnionWith(user.Permissions.Select(permission => permission.Description));

        return permissions;
    }

    public Task<HashSet<string>> GetPermissionsForUserAsync(Guid userId)
    {
        return GetForUserIdAsync(userId);
    }

    public HashSet<string> GetPermissionsForRole(string roleName)
        => _rolePermissions.TryGetValue(roleName, out var permissions)
            ? permissions()
            : [];

    public async Task<bool> UserHasPermissionAsync(Guid userId, string permission)
    {
        try
        {
            var permissions = await GetForUserIdAsync(userId);
            return permissions.Contains(permission);
        }
        catch (TickestException ex)
        {
            logger.LogError(ex, "Erro ao verificar permissão para o usuário.");
            return false;
        }
    }

    public async Task ValidatePermissionAsync(Guid userId, string permission)
    {
        var hasPermission = await UserHasPermissionAsync(userId, permission);

        if (!hasPermission)
        {
            logger.LogError("Usuário {UserId} não tem permissão para {Permission}.", userId, permission);
            throw new TickestException($"Usuário não tem permissão para {permission}.");
        }

        logger.LogInformation("Usuário {UserId} tem permissão para a ação {Permission}.", userId, permission);
    }

    public Task<bool> CanUserLoginAsync(Guid userId)
    {
        return UserHasPermissionAsync(userId, SystemPermissions.AccessSystem);
    }

    private static HashSet<string> GetAdminMasterPermissions() =>
    [
        SystemPermissions.FullSystemControl,
        SystemPermissions.ManageUsers,
        SystemPermissions.DeleteUser,
        SystemPermissions.ManagePermissions,
        SystemPermissions.ManageTickets,
        SystemPermissions.DeleteTicket,
        SystemPermissions.CloseTicket,
        SystemPermissions.ReopenTicket,
        SystemPermissions.AssignTicket,
        SystemPermissions.ChangeTicketStatus,
        SystemPermissions.CreateTicket,
        SystemPermissions.ViewTicket,
        SystemPermissions.UpdateOwnTicket,
        SystemPermissions.ViewReports,
        SystemPermissions.AccessCriticalSettings,
        SystemPermissions.AccessSystem
    ];

    private static HashSet<string> GetAdminPermissions() =>
    [
        SystemPermissions.AccessSystem,
        SystemPermissions.ManageUsers,
        SystemPermissions.CreateTicket,
        SystemPermissions.ViewTicket,
        SystemPermissions.AssignTicket,
        SystemPermissions.UpdateOwnTicket,
        SystemPermissions.CloseTicket
    ];

    private static HashSet<string> GetTicketManagerPermissions() =>
    [
        SystemPermissions.ManageTickets,
        SystemPermissions.DeleteTicket,
        SystemPermissions.CloseTicket,
        SystemPermissions.ReopenTicket,
        SystemPermissions.AssignTicket,
        SystemPermissions.ChangeTicketStatus,
        SystemPermissions.ViewTicket,
        SystemPermissions.ViewReports,
        SystemPermissions.AccessSystem
    ];

    private static HashSet<string> GetCollaboratorPermissions() =>
    [
        SystemPermissions.CreateTicket,
        SystemPermissions.ViewTicket,
        SystemPermissions.UpdateOwnTicket,
        SystemPermissions.TrackTicketStatus,
        SystemPermissions.InteractWithTicket,
        SystemPermissions.AccessSystem
    ];

    private static Dictionary<string, Func<HashSet<string>>> InitializeRolePermissions() =>
        new()
        {
            [SystemRoles.AdminMaster] = GetAdminMasterPermissions,
            [SystemRoles.Admin] = GetAdminPermissions,
            [SystemRoles.TicketManager] = GetTicketManagerPermissions,
            [SystemRoles.Collaborator] = GetCollaboratorPermissions
        };
}
