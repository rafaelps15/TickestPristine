using Tickest.Application.Abstractions.Authentication;
using Microsoft.Extensions.Logging;

namespace Tickest.Infrastructure.Authorization;

public class PermissionValidator
{
    private readonly IAuthService _authService;
    private readonly IPermissionProvider _permissionProvider;
    private readonly ILogger<PermissionValidator> _logger;

    #region Construtor

    public PermissionValidator(
        IAuthService authService,
        IPermissionProvider permissionProvider,
        ILogger<PermissionValidator> logger)
        => (_authService, _permissionProvider, _logger) = (authService, permissionProvider, logger);

    #endregion

    public async Task<bool> HasRequiredPermissionsAsync(Guid userId, HashSet<string> requiredPermissions, CancellationToken cancellationToken)
    {
        // Tenta obter o usuário atual de maneira simplificada
        var currentUser = await _authService.GetCurrentUserAsync(cancellationToken);

        // Se não encontrar o usuário ou o ID não corresponder, retorna false e loga
        if (currentUser?.Id != userId)
        {
            _logger.LogWarning("Usuário não encontrado ou não autorizado.");
            return false;
        }

        try
        {
            // Obtém as permissões do usuário
            var userPermissions = await _permissionProvider.GetPermissionsForUserAsync(userId);

            // Verifica se o usuário possui todas as permissões requeridas
            return requiredPermissions.IsSubsetOf(userPermissions);
        }
        catch (Exception ex)
        {
            // Loga qualquer erro ocorrido
            _logger.LogError(ex, "Erro ao validar permissões do usuário.");
            return false;
        }
    }
}
