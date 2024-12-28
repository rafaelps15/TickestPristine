using Microsoft.AspNetCore.Authorization;

namespace Infrastructure.Authorization;

#region "Atributo de Permissão"

/// <summary>
/// Atributo para aplicar a autorização baseada em permissão em controladores ou métodos.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    /// <summary>
    /// Construtor que define a política de permissão para o atributo.
    /// </summary>
    /// <param name="permission">A permissão necessária para acessar o recurso.</param>
    public HasPermissionAttribute(string permission) => Policy = permission; // Atribui a permissão diretamente

}

#endregion
