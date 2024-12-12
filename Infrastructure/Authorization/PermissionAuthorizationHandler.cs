using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Threading;
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
    /// <summary>
    /// Verifica se o usuário possui a permissão necessária.
    /// </summary>
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
    {
        var currentUser = await _authService.GetCurrentUserAsync(CancellationToken.None);

        if (currentUser == null || !await _permissionProvider.UserHasPermissionAsync(currentUser.Id, requirement.Permission))
        {
            _logger.LogWarning($"Permissão '{requirement.Permission}' negada ao usuário {currentUser?.Id}.");
            context.Fail();
            return;
        }

        _logger.LogInformation($"Permissão '{requirement.Permission}' concedida ao usuário {currentUser?.Id}.");
        context.Succeed(requirement);
    }
}