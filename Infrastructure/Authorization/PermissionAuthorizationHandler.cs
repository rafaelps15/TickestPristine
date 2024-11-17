using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Tickest.Application.Abstractions.Authentication;

namespace Infrastructure.Authorization;

internal sealed class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
{
    private readonly ILogger<PermissionAuthorizationHandler> _logger;
    private readonly IPermissionProvider _permissionProvider;
    private readonly Dictionary<Guid, HashSet<string>> _userPermissionsCache = new();
    private readonly IAuthService _authService;

    public PermissionAuthorizationHandler(
        ILogger<PermissionAuthorizationHandler> logger,
        IPermissionProvider permissionProvider,
        IAuthService authService)
    {
        (_logger, _permissionProvider) = (logger, permissionProvider);
        _authService = authService;
    }

    /// <summary>
    /// Verifica se o usuário possui a permissão necessária.
    /// </summary>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var currentUser = await _authService.GetCurrentUserAsync();

        // Verificação do usuário
        if (currentUser is null)
        {
            _logger.LogWarning("Não autorizado: Usuário não autenticado.");
            context.Fail();
            return;
        }

        // Verifica se o usuário possui a permissão necessária
        var permissions = await GetPermissionsAsync(currentUser.Id);
        if (permissions.Contains(requirement.Permission))
        {
            _logger.LogInformation($"Permissão '{requirement.Permission}' concedida ao usuário {currentUser.Id}.");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogWarning($"Permissão '{requirement.Permission}' negada ao usuário {currentUser.Id}.");
            context.Fail();
        }
    }

    private async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        if (_userPermissionsCache.TryGetValue(userId, out var cachedPermissions))
        {
            return cachedPermissions;
        }

        // Carrega as permissões e as armazena no cache
        var permissions = await CachePermissions(userId);
        return permissions;
    }

    private async Task<HashSet<string>> CachePermissions(Guid userId)
    {
        var permissions = await _permissionProvider.GetPermissionsForUserAsync(userId);
        _userPermissionsCache[userId] = permissions;
        return permissions;
    }
}
