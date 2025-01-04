//using Microsoft.Extensions.Logging;
//using Tickest.Application.Abstractions.Authentication;
//using Tickest.Domain.Entities.Users;
//using Tickest.Domain.Exceptions;

//namespace Tickest.Infrastructure.Authorization;

//public class PermissionValidator
//{
//    private readonly IAuthService _authService;
//    private readonly IPermissionProvider _permissionProvider;
//    private readonly ILogger<PermissionValidator> _logger;

//    #region Construtor

//    public PermissionValidator(
//        IAuthService authService,
//        IPermissionProvider permissionProvider,
//        ILogger<PermissionValidator> logger)
//        => (_authService, _permissionProvider, _logger) = (authService, permissionProvider, logger);

//    #endregion

//    // Verifica se o usuário possui todas as permissões requeridas
//    public async Task<bool> HasRequiredPermissionsAsync(Guid userId, HashSet<string> requiredPermissions, CancellationToken cancellationToken)
//    {
//        // Obtém as permissões do usuário
//        var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(userId);

//        // Verifica se o usuário possui todas as permissões requeridas
//        return requiredPermissions.IsSubsetOf(userPermissions);
//    }

//    // Valida se o usuário tem permissão para realizar uma ação específica
//    public async Task ValidatePermissionAsync(User currentUser, string permission, CancellationToken cancellationToken)
//    {
//        if (currentUser == null)
//        {
//            _logger.LogError("Usuário não autenticado.");
//            throw new TickestException("Usuário não autenticado.");
//        }

//        var permissions = await _permissionProvider.GetPermissionsForUserAsync(currentUser.Id);

//        if (!permissions.Contains(permission))
//        {
//            _logger.LogError("Usuário {UserId} não tem permissão para {Permission}.", currentUser.Id, permission);
//            throw new TickestException($"Usuário não tem permissão para {permission}.");
//        }

//        _logger.LogInformation("Usuário {UserId} tem permissão para {Permission}.", currentUser.Id, permission);
//    }
//}
