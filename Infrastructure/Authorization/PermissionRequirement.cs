using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

#region "Requisito de Permissão"

internal sealed class PermissionRequirement : IAuthorizationRequirement
{
    public PermissionRequirement(string permission) =>
        Permission = permission; // Inicializa a propriedade de permissão

    /// <summary>
    /// Propriedade que armazena a permissão necessária.
    /// </summary>
    public string Permission { get; }
}

#endregion
