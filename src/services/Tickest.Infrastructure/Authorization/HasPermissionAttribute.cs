using Microsoft.AspNetCore.Authorization;

namespace Tickest.Infrastructure.Authorization;

#region "Atributo de Permissão"

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class HasPermissionAttribute : AuthorizeAttribute
{
    public HasPermissionAttribute(string permission) => Policy = permission; // Atribui a permissão diretamente

}

#endregion
