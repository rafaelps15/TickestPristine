using Tickest.Application.Abstractions.Authentication;
using Microsoft.Extensions.Logging;

namespace Tickest.Infrastructure.Authorization;

public class PermissionValidator
{
    private readonly IAuthService _authService;
    private readonly IPermissionProvider _permissionProvider;
    private readonly ILogger<PermissionValidator> _logger;

    public PermissionValidator(
        IAuthService authService,
        IPermissionProvider permissionProvider,
        ILogger<PermissionValidator> logger) =>
        (_authService, _permissionProvider, _logger) = (authService, permissionProvider, logger);

    public async Task<bool> HasRequiredPermissionsAsync(Guid userId, HashSet<string> requiredPermissions, CancellationToken cancellationToken)
    {
        try
        {
            var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

            if (currentUser == null || currentUser.Id != userId)
            {
                LogUnauthorizedUser();
                return false;
            }

            var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(userId);
            return userPermissions?.Any() == true && requiredPermissions.All(permission => userPermissions.Contains(permission));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar permissões do usuário.");
            return false;
        }
    }

    private void LogUnauthorizedUser()
    {
        _logger.LogError("Usuário não encontrado ou não autorizado.");
    }
}


