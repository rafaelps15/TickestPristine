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

    private static HashSet<string> GetAdminMasterPermissions() => new()
    {
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
    };

    private static HashSet<string> GetTicketManagerPermissions() => new()
    {
        SystemPermissions.ManageTickets,
        SystemPermissions.DeleteTicket,
        SystemPermissions.CloseTicket,
        SystemPermissions.ReopenTicket,
        SystemPermissions.AssignTicket,
        SystemPermissions.ChangeTicketStatus,
        SystemPermissions.ViewTicket,
        SystemPermissions.ViewReports,
        SystemPermissions.AccessSystem
    };

    private static HashSet<string> GetCollaboratorPermissions() => new()
    {
        SystemPermissions.CreateTicket,
        SystemPermissions.ViewTicket,
        SystemPermissions.UpdateOwnTicket,
        SystemPermissions.TrackTicketStatus,
        SystemPermissions.InteractWithTicket,
        SystemPermissions.AccessSystem
    };

    private static Dictionary<string, Func<HashSet<string>>> InitializeRolePermissions() =>
        new()
        {
            [SystemRoles.AdminMaster] = GetAdminMasterPermissions,
            [SystemRoles.TicketManager] = GetTicketManagerPermissions,
            [SystemRoles.Collaborator] = GetCollaboratorPermissions
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
        return await UserHasPermissionAsync(userId, SystemPermissions.AccessSystem);
    }
}
